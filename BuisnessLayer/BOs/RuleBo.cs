//using SimulatorLD.DBLayer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using QuickFix;
using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;


namespace SimulatorLD.BuisnessLayer.BOs
{
    public  class RuleBo
    {
        private DBLayer.DAOs.Rule _rule; 
        private RuleRepo rulerepo;
        private RulesManagementDbContext _context;

        
        public RuleBo()
        {
            rulerepo = new DBLayer.Repository.RuleRepo();
        }

        public List<DBLayer.DAOs.Rule> GetAllRules()
        {

            return  rulerepo.GetAllRules();
        }

        public void AddRule(DBLayer.DAOs.Rule rule)
        {
            rulerepo.AddRule(rule);
          
        }



        //public virtual void ProcessOrder(DBLayer.DAOs.Order order) { }



        /*public bool IsMatching(DBLayer.DAOs.Order order)


        *//*The idea :  test the matching with every rule attribute  *//*
        {
            if (!string.IsNullOrEmpty(_rule.Symbol) && _rule.Symbol != order.Symbol)
            {
                return false;
            }


            if (_rule.MinPrice.HasValue && float.Parse(order.Price) < _rule.MinPrice.Value)
            {
                return false;
            }

            if (_rule.MaxPrice.HasValue && float.Parse(order.Price) > _rule.MaxPrice.Value)
            {
                return false;
            }

            if (_rule.MinQty.HasValue && float.Parse(order.OrderQuantity) < _rule.MinQty.Value)
            {
                return false;
            }

            if (_rule.MaxQty.HasValue && float.Parse(order.OrderQuantity) > _rule.MaxQty.Value)
            {
                return false;
            }

            return true;


        }*/

        /*public bool IsOrderMatchingAnyRule(Models.Order order)
        {
            var rules = _rule.GetAllRules();
            /*The idea : list all rules from the DB and apply the IsMatchingfunction */


        /*Methode qui block l'execution ("send a block execution message") selon le type de rule et le matching de l'ordre #*/
        /*public void ProcessOrder(DBLayer.DAOs.Order order)
        {
            var rules = rulerepo.GetAllRules();
            foreach (var rule in rules)
            {
                if (IsMatching(order))
                {   
                    if (_rule.RuleType == RuleTypesEnum.BlockSecurityRule ) { }
                    //SendBlockExecutionMessage(Message);
                    if (_rule.RuleType == RuleTypesEnum.ExecuteAllRule) { }
                    //SendAllowExecutionMessage(Message);
                    if (_rule.RuleType == RuleTypesEnum.ExecutePartially) { }
                    //SendPartiallyExecutionMessage(Message);
                    if (_rule.RuleType == RuleTypesEnum.ExecuteIncrementally) { }
                   //SendIncrementalExecutionMessage(Message);






                }
            }
            
        }*/
    }
}
