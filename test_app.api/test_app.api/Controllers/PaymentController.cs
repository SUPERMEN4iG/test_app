using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using test_app.api.Helper.Hash;
using test_app.api.Logic;
using test_app.api.Models;
using test_app.shared;
using test_app.shared.Data;
using test_app.shared.Repositories;
using test_app.shared.ViewModels;

namespace test_app.api.Controllers
{
    [Produces("application/json")]
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IG2ARepository _G2ARepository;
        private readonly IUnitOfWork<ApplicationDbContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        private const string API_HASH = "049eb318-e3a2-4724-a869-c3355b5fa73a";
        private const string MERCHANT_EMAIL = "supermen4ig988@gmail.com";
        private const string API_SECRET = "X6fH81H=sNvP7UK~^ZH$tK0ussFi-~*L&ec6fu%M5rlB9!46SL19f1=Lt5=h^_=$";

        public PaymentController(
            UserManager<ApplicationUser> userManager,
            IG2ARepository g2ARepository,
            IUserRepository userRepository,
            IUnitOfWork<ApplicationDbContext> unitOfWork)
        {
            _userManager = userManager;
            _G2ARepository = g2ARepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("getg2arefilldata")]
        [HttpGet]
        public async Task<IActionResult> GetG2ARefillData(decimal amount)
        {
            try
            {
                if (amount < 3)
                {
                    throw new Exception("Minimum amount is $3 USD");
                }

                var g2aPayment = new G2APayment();
                g2aPayment.User = await _userManager.GetUserAsync(HttpContext.User);
                g2aPayment.Sum = amount;
                g2aPayment.Currency = "USD";
                g2aPayment.Status = G2APayment.G2APaymentStatus.None;

                _G2ARepository.Add(g2aPayment);
                _unitOfWork.SaveChanges();

                var hashComputed = String.Format("{0}{1}{2}{3}", 
                    g2aPayment.Id, 
                    Math.Round(Convert.ToDecimal(g2aPayment.Sum), 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture), 
                    g2aPayment.Currency, 
                    API_SECRET);
                hashComputed = SHA256Helper.ComputeHash(hashComputed);

                var pData = new G2ACreateQuoteRequest()
                {
                    api_hash = API_HASH,
                    hash = hashComputed,
                    order_id = g2aPayment.Id,
                    amount = Math.Round(Convert.ToDecimal(g2aPayment.Sum), 2).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture),
                    currency = g2aPayment.Currency,
                    url_failure = "http://eachskins.com/",
                    url_ok = "http://eachskins.com/",
                    security_user_logged_in = 1,
                    security_steam_id = g2aPayment.User.Id,
                    items = new List<G2ACreateQuoteRequest.G2ACreateQuoteRequestItem>() {
                        new G2ACreateQuoteRequest.G2ACreateQuoteRequestItem() {
                            sku = 1,
                            name = "EachSkins Balance",
                            amount = Math.Round(Convert.ToDecimal(g2aPayment.Sum), 2),
                            qty = 1,
                            id = 1,
                            price = Math.Round(Convert.ToDecimal(g2aPayment.Sum), 2),
                            url = "http://eachskins.com/"
                        }
                    }
                };

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://checkout.test.pay.g2a.com/index/createQuote"));
                webRequest.Method = "POST";
                webRequest.Headers.Add(HttpRequestHeader.Accept, "application/json");
                webRequest.ContentType = "application/x-www-form-urlencoded";
                DataContractJsonSerializer ser = new DataContractJsonSerializer(pData.GetType());

                StringBuilder sb = new StringBuilder();
                foreach (var propertie in pData.GetType().GetProperties())
                {
                    var value = propertie.GetValue(pData);
                    var key = propertie.Name;

                    if (sb.Length > 0) sb.Append('&');
                    sb.Append(HttpUtility.UrlEncode(key));
                    sb.Append('=');

                    if (key != "items")
                    {
                        sb.Append(value);
                    }
                    else
                    {
                        sb.Append(JsonConvert.SerializeObject(value));
                    }
                }

                var data = Encoding.UTF8.GetBytes(sb.ToString(), 0, sb.Length);
                webRequest.ContentLength = data.Length;
                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                WebResponse response = await webRequest.GetResponseAsync();

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    String responseString = reader.ReadToEnd();
                    return Json(BaseHttpResult.GenerateSuccess(responseString, "success", ResponseType.Ok));
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(WebException))
                {
                    WebResponse errResp = ((WebException)ex).Response;
                    using (Stream respStream = errResp.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(respStream);
                        string text = reader.ReadToEnd();
                        return BadRequest(BaseHttpResult.GenerateError(String.Format("{0}", text), ResponseType.ServerError));
                    }
                }
                else
                {
                    return BadRequest(BaseHttpResult.GenerateError(String.Format("{0}", ex.Message.ToString()), ResponseType.ServerError));
                }
            }
        }

        [HttpPost]
        [Route("g2arefill")]
        public IActionResult G2ARefillIPN(G2ARefillViewModel pData)
        {
            var vertifiedHash = SHA256Helper.ComputeHash(
                String.Format("{0}{1}{2}{3}",
                    pData.transactionId,
                    pData.userOrderId,
                    pData.amount,
                    API_SECRET));

            var req = JsonConvert.SerializeObject(pData).ToString();
            var res = "";

            using (var transaction = _unitOfWork.Context.Database.BeginTransaction())
            {
                try
                {
                    if (vertifiedHash != pData.hash)
                    {
                        throw new Exception("hash validation error");
                    }

                    if (pData.status != "complete")
                    {
                        throw new Exception("status is not complete");
                    }

                    G2APayment g2APayment = _G2ARepository.GetPayment(pData.userOrderId);
                    ApplicationUser user = _userRepository.Get(x => x.Id == g2APayment.User.Id).FirstOrDefault();

                    if (g2APayment.Status == G2APayment.G2APaymentStatus.Success)
                    {
                        throw new Exception("status is already complete");
                    }

                    user.Balance += pData.amount;
                    g2APayment.Status = G2APayment.G2APaymentStatus.Success;
                    g2APayment.TransactionId = pData.transactionId;

                    _G2ARepository.Update(g2APayment);
                    _userRepository.Update(user);

                    // TODO: Временные логи, на момент теста
                    res = "success";
                    _G2ARepository.AddIpnLog(req, res);

                    _unitOfWork.SaveChanges();
                    transaction.Commit();

                    return Json(BaseHttpResult.GenerateSuccess(g2APayment.Status, "success", ResponseType.Ok));
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    // TODO: Временные логи, на момент теста
                    res = ex.Message.ToString();
                    res += '|' + ex.StackTrace;

                    _G2ARepository.AddIpnLog(req, res);
                    _unitOfWork.SaveChanges();

                    return BadRequest(BaseHttpResult.GenerateError(ex.Message, ResponseType.ServerError));
                }
            }
        }

        public class G2ARefillViewModel
        {
            public string type { get; set; }

            public string transactionId { get; set; }

            public Int64 userOrderId { get; set; }

            public decimal amount { get; set; }

            public string currency { get; set; }

            public string status { get; set; }

            public string orderCreatedAt { get; set; }

            public string orderCompleteAt { get; set; }

            public decimal refundedAmount { get; set; }

            public decimal provisionAmount { get; set; }

            public string hash { get; set; }
        }

        [Serializable]
        public class G2ACreateQuoteRequest
        {
            public class G2ACreateQuoteRequestItem
            {
                public int sku { get; set; }

                public string name { get; set; }

                public decimal amount { get; set; }

                public int qty { get; set; }

                public int id { get; set; }

                public decimal price { get; set; }

                public string url { get; set; }
            }

            public string api_hash { get; set; }

            public string hash { get; set; }

            public Int64 order_id { get; set; }

            public string amount { get; set; }

            public string currency { get; set; }

            public string url_failure { get; set; }

            public string url_ok { get; set; }

            public int security_user_logged_in { get; set; }

            public string security_steam_id { get; set; }

            public List<G2ACreateQuoteRequestItem> items { get; set; }
        }
    }
}
