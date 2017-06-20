using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LRWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contract()
        {
            return View();
        }

        // publish
        public async Task<bool> RegisterContract([FromBody] ContractRegisterData data)
        {
            ProfileContract profile = new ProfileContract();
            await profile.RegisterContract(data.States);
            return true;
        }

        // approve & initiate
        public async Task<bool> InitiateContract([FromBody] ContractData data)
        {
            ProfileContract profile = new ProfileContract();
            await profile.Intantiate(data.State, data.FormCode, data.Comments, data.Approver);
            return true;
        }
    }
}
