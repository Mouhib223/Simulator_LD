using SimulatorLD.BuisnessLayer.BOs;
using SimulatorLD.BuisnessLayer.Models;
using SimulatorLD.DBLayer.DAOs;

namespace SimulatorLD.BuisnessLayer
{
    public class RuleManager
    {
       

            private List<RuleBo> _rules;

            public RuleManager()
            {
                _rules = new List<RuleBo> ();
            }


           /* public void LoadFromDatabase()
            {


                List<DBLayer.DAOs.Rule> daoRules = new List<DBLayer.DAOs.Rule>();
                ///


                foreach (var daoRule in daoRules)
                {
                    
                    switch ((RuleTypesEnum)daoRule.RuleType)

                    {
                        case DBLayer.DAOs.RuleTypesEnum.BlockSecurityRule:
                            _rules.Add(new BlockSecurityRule(daoRule));
                            break;
                    }


                }

            }*/


            public void Sync()
            {
                _rules.Clear();
                //LoadFromDatabase();
            }







           /* public void ProcessOrder(DBLayer.DAOs.Order order)
            {
                foreach (RuleBo rule in _rules)
                {
                    if (rule.IsMatching(order))
                    {
                       // rule.ProcessOrder(order);
                    }*
                }
            }*/

        }
    }

