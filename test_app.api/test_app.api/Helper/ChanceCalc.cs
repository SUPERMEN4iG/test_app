using System.Collections.Generic;
using System.Linq;
using test_app.api.Data;
using test_app.api.Models;

namespace test_app.api.Helper {
    public class ChanceCalc {
        private int _skin_limit { get; set; }

        private int _limit = 100000;
        private int iteration;
        private double _marginality;

        private double _precision;

        private double _casePrice;

        private ApplicationDbContext _context;

        private List<SkinViewModel> _fakeInventory;
        private List<SkinViewModel> _ignoredSkins;
        private double _skin_max_chance;

        private List<SkinViewModel> _skinsPool;

        public ChanceCalc (ApplicationDbContext context) {
            _context = context;
            iteration = 0;
            _precision = 0.01;
            _skin_max_chance = 4;
        }

        public List<SkinViewModel> Calc (int case_id, double case_price,double margine, List<long> skinIds) {
            _marginality = margine;
            _casePrice = case_price;
            _fakeInventory = _context.CasesDrops.Where(x => x.CaseId == case_id && !skinIds.Contains(x.SkinId)).Select(skin => new SkinViewModel()
            {
                Id = skin.SkinId,
                Price = skin.Skin.Price * 0.8M
            }).ToList ();
            _ignoredSkins = _context.CasesDrops.Where(x => x.CaseId == case_id && skinIds.Contains(x.SkinId)).Select(skin => new SkinViewModel()
            {
                Id = skin.SkinId,
                Price = skin.Skin.Price,
                Chance = 0
            }).ToList();

            _skinsPool = new List<SkinViewModel>(_fakeInventory); // probably fixes
            push_skins ();
            _skinsPool.ForEach (skin => {
                skin.Chance = System.Math.Round (skin_chances (skin.Id), 6) / 100;
            });
            _skinsPool.Concat(_ignoredSkins);
            return _skinsPool;

        }

        private double calc_selling () {
            return _fakeInventory.Count * _casePrice;
        }

        private double calc_buying () {

            return _fakeInventory.Sum(x => (double)x.Price);

        }

        private double calc_marginality () {
            return 100 - (calc_buying () / calc_selling () * 100);
        }

        private int skin_count (long skin_id) {
            return _fakeInventory.Count (x => x.Id == skin_id);
        }

        private double skin_chances (long skin_id) {
            /* Считаем частоту его выпадения. Для этого кол-во этого скина нужно поделить на общее кол-во предметов
             * Например: 1 m4a4 из 10 других скинов, частота выпадения - 10% (1 / 10 * 100);
             */
            return (double)skin_count (skin_id) / _fakeInventory.Count * 100;
        }

        private bool can_skin_be_pushed (long skin_id) {
            return skin_chances (skin_id) <= _skin_max_chance;
        }

        private void push_skins () {
           
            this.iteration++;

            if (this.iteration > _limit) {
                return;
            }

            bool found = false;
            bool pushed_any = false;

            _skinsPool.ForEach (skin => {
                if (!can_skin_be_pushed (skin.Id)) {
                    return;
                }
                var marginality_before = this.calc_marginality ();
                /* Если сейчас разница марж > 0, то нам нужен предмет подешевле, наша маржа в минусе
                 * Если сейчас разница марж < 0, то нам нужен предмет подороже, наша маржа в плюсе
                 * Если в диапозоне погрешности, то выходим из рекурсии
                 */
                var marginality_diff = this._marginality - marginality_before;
                if (marginality_diff <= _precision && marginality_diff >= -_precision) {
                    found = true;
                    return;
                }
                if (marginality_diff > 0 && skin.Price < (decimal)_casePrice) {
                    _fakeInventory.Add (skin);
                    pushed_any = true;
                }
                if (marginality_diff < 0 && skin.Price > (decimal)_casePrice) {
                    _fakeInventory.Add (skin);
                    pushed_any = true;
                }
            });
            if (!found) {
                if (!pushed_any) {
                    _skin_max_chance += 0.5;
                }
                push_skins ();
            }

        }

    }
}