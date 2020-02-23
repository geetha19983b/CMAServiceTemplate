using CMAService.Repository;
using System;

namespace CMAService.Business
{
    public class BusinessAccess : IBusinessAccess
    {
        IDataAccess dataAccess;
        public BusinessAccess(IDataAccess _dataAccess)
        {
            dataAccess = _dataAccess;
        }

    }

}
