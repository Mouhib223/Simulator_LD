using Microsoft.EntityFrameworkCore;
using SimulatorLD.DBLayer.DAOs;

namespace SimulatorLD.DBLayer.Repository
{
    public class RuleRepo
    {

        public List<Rule> GetAllRules()
        {
            var db = new RulesManagementDbContext();
            return db.Rules.ToList();
        }


        /*  public string getSymbol()
          { return(getSymbol()); }*/
        /*public Rule GetRuleBySymbol(string symbol)
        {
            var db = new RulesManagementDbContext();
            Rule rule = new Rule();
            rule = db.Rules.FirstOrDefault(x => x.Symbol == symbol);
            if (rule == null)
                throw new Exception("NotFound");
            return rule;

        }*/

        public Rule GetRuleById(int Id)
        {
            var db = new RulesManagementDbContext();
            Rule rule = new Rule();
            rule = db.Rules.FirstOrDefault(x => x.RuleId == Id);
            if (rule == null)
                throw new Exception("NotFound");
            return rule;

        }
        public Rule GetRuleBySymbol(string symbol)
        {
            var db = new RulesManagementDbContext();
            Rule rule = db.Rules.FirstOrDefault(x => x.Symbol == symbol);
            if (rule == null)
                throw new Exception("NotFound");
            return rule;
        }
        public void UpdateRule(Rule rule)
        {
            var db = new RulesManagementDbContext();
            var existingRule = db.Rules.FirstOrDefault(x => x.RuleId == rule.RuleId);
            if (existingRule == null)
                throw new Exception("NotFound");

            // Update the existing rule properties
            existingRule.RuleType = rule.RuleType;
            existingRule.Symbol = rule.Symbol;
            existingRule.MinPrice = rule.MinPrice;
            existingRule.MaxPrice = rule.MaxPrice;
            existingRule.MinQty = rule.MinQty;
            existingRule.MaxQty = rule.MaxQty;
            existingRule.Description = rule.Description;

            db.SaveChanges();
        }

        public void DeleteRule(int ruleId)
        {
            var db = new RulesManagementDbContext();
            var rule = db.Rules.FirstOrDefault(x => x.RuleId == ruleId);
            if (rule == null)
                throw new Exception("NotFound");

            db.Rules.Remove(rule);
            db.SaveChanges();
        }
        public void AddRule(Rule rule)
        {
            var db = new RulesManagementDbContext();
            rule.RuleType = Enum.Parse<RuleTypesEnum>(rule.RuleType.ToString());
            db.Rules.Add(rule);
            db.SaveChanges();
        }
         
       

        /*public void UpdateRule(Rule rule) 
        {
            *//*var db = new RulesManagementDbContext();
            Rule existingRule = db.Rules.FindAsync<R>();
            if (existingRule != null)
            {
                = rule.RuleType;
                existingRule.Symbol = rule.Symbol;
                existingRule.MinPrice = rule.MinPrice;
                existingRule.MaxPrice = rule.MaxPrice;
                existingRule.MinQty = rule.MinQty;
                existingRule.MaxQty = rule.MaxQty;
                existingRule.Description = rule.Description;
                await _context.SaveChangesAsync();
            }*//*

        }
        public void DeleteRule(Rule rule)
        {
            var db = new RulesManagementDbContext();
            db.Rules.Remove(rule);
            db.SaveChanges();

        }*/

    }
}
