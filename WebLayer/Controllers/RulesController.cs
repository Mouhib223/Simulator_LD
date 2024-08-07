using Microsoft.AspNetCore.Mvc;
using SimulatorLD.BuisnessLayer.BOs;
using SimulatorLD.DBLayer.DAOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimulatorLD.WebLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RulesController : ControllerBase 
    {
        private RuleBo _bo;

        public RulesController()
        {
            _bo = new BuisnessLayer.BOs.RuleBo();
        }

        [HttpGet]
        [Route("RulesList")]
        public List<Rule> GetAllRules()
        {
            return _bo.GetAllRules();
        }

        [HttpPost]
        [Route("AddRule")]
        public void AddRule(Rule rule)
        {
            _bo.AddRule(rule);

        }
        [HttpPut]
        [Route("UpdateRule")]
        public IActionResult UpdateRule(Rule rule)
        {
            try
            {
                _bo.UpdateRule(rule);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteRule/{id}")]
        public IActionResult DeleteRule(int id)
        {
            try
            {
                _bo.DeleteRule(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetRuleById/{id}")]
        public IActionResult GetRuleById(int id)
        {
            try
            {
                var rule = _bo.GetRuleById(id);
                return Ok(rule);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}

