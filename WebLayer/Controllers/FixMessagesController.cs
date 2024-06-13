using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimulatorLD.BuisnessLayer.BOs;
using SimulatorLD.BuisnessLayer.Models;
using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;

namespace SimulatorLD.WebLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FixMessagesController : ControllerBase
    {

        private FixMessageBo _Bo;
        public FixMessagesController()
        { 
           _Bo=new BuisnessLayer.BOs.FixMessageBo();
         }


        [HttpGet]
        [Route("RecievedMessages")]
        public List<Fixmessage> GetAllMessages()
        {
            return _Bo.GetAllMessages();
        }

        [HttpPost]
        [Route("AddMessage")]
        public void AddMessage(Fixmessage message)
        {
            _Bo.AddMessage(message);

        }




    }
}
