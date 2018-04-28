using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using test_app.api.Logic.LastWinnersSocket;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/main")]
    public class MainController : Controller
    {
        private LastWinnersHandler _lastWinnersHandler { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IWinnerRepository _winnerRepository;

        public MainController(
            LastWinnersHandler lastWinnersHandler,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IWinnerRepository winnerRepository)
        {
            _lastWinnersHandler = lastWinnersHandler;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _winnerRepository = winnerRepository;
        }

        [HttpGet]
        [Route("getdata")]
        public IActionResult GetData()
        {
            var result = new GetDataResult();

            result.OpennedCases = 0L;
            result.UsersRegistered = 0L;
            result.Online = 0;
            result.LastWinners = new List<LastWinnersViewModel>();

            try {
                result.OpennedCases = _winnerRepository.GetWinnersCount();
                result.UsersRegistered = _userRepository.GetUsersCount();

                result.LastWinners.AddRange(_winnerRepository.GetLastWinners());

                result.Online = _lastWinnersHandler.GetCountConnections();
                result.ServerTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                // TODO: Logs to need
            }

            return Json(result);
        }
    }

    public class GetDataResult
    {
        public Int64 OpennedCases { get; set; }
        public Int64 UsersRegistered { get; set; }
        public Int64 Online { get; set; }
        public DateTime ServerTime { get; set; }
        public List<LastWinnersViewModel> LastWinners { get; set; }
    }
}