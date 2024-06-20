using SimulatorLD.DBLayer.DAOs;
using SimulatorLD.DBLayer.Repository;

namespace SimulatorLD.BuisnessLayer.BOs
{
    public class FixMessageBo
    {

        private FixMessageRepo _msg;

        public FixMessageBo()
        {
            _msg = new DBLayer.Repository.FixMessageRepo();
        }

        public List<Fixmessage> GetAllMessages()
        {

            return _msg.GetAllMessages();
        }

        public void AddMessage(Fixmessage msg)
        {
            _msg.AddMessage(msg);


        }

    }
}
