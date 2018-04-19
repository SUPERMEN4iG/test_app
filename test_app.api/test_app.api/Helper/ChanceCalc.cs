using System;
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

        private decimal _totalPriceBuying = 0;

        private Dictionary<long,int> _skinsCount;

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
            _skinsPool = _context.CasesDrops.Where(x => x.CaseId == case_id && !skinIds.Contains(x.SkinId)).Select(skin => new SkinViewModel()
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

            _fakeInventory = new List<SkinViewModel>(); // probably fixes

            _skinsCount = _skinsPool.ToDictionary(k=>k.Id,k=>0);

        var maxPrice = _skinsPool.Max(x=>x.Price);

         
           
    _skinsPool.ForEach(skin=>{
        var times_to_add = 0;
        if((double)skin.Price > this._casePrice * 0.6)
        {
            var t = maxPrice / skin.Price;
            times_to_add = (int)Math.Round(t);
            if (times_to_add < 1) 
            {
                times_to_add = 1;
            }
        }
        else{
            times_to_add = 1;
        }

        for(var i = 0; i < times_to_add; i++)
        {
            _fakeInventory.Add(skin);
            _skinsCount[skin.Id]++;
            this._totalPriceBuying +=skin.Price;
        }
        
    });
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

            return (double)_totalPriceBuying;

        }

        private double calc_marginality () {
            return 100 - (calc_buying () / calc_selling () * 100);
        }

        private int skin_count (long skin_id) {
            return _skinsCount[skin_id];
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
                     _skinsCount[skin.Id]++;
                    this._totalPriceBuying +=skin.Price;
                    pushed_any = true;
                }
                if (marginality_diff < 0 && skin.Price > (decimal)_casePrice) {
                    _fakeInventory.Add (skin);
                     _skinsCount[skin.Id]++;
                    this._totalPriceBuying +=skin.Price;
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