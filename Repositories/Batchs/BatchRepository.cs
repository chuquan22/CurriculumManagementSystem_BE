using BusinessObject;
using DataAccess.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Batchs
{
    public class BatchRepository : IBatchRepository
    {
        private readonly BatchDAO _batchDAO = new BatchDAO();
        public List<Batch> GetAllBatch()
        {
            return _batchDAO.GetAllBatch(); 
        }
    }
}
