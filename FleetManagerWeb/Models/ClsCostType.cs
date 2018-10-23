using FleetManagerWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FleetManagerWeb.Models
{
    public class ClsCostType : IClsCostType
    {
        private AdditionalCostsDataContext objDataContext = null;

        public bool HdniFrame { get; set; }

        public long Id { get; set; }

        public List<ClsCostType> TypesList { get; set; }

        public string TypeName { get; set; }

        public long SaveCostType(ClsCostType objSave)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (this.objDataContext = new AdditionalCostsDataContext(Functions.StrConnection))
                    {
                        var result = objDataContext.InsertOrUpdateTripCostType(objSave.Id, objSave.TypeName, mySession.Current.UserId, PageMaster.AdditionalCost)
                            .FirstOrDefault();

                        if (result != null)
                        {
                            objSave.Id = result.InsertedId;
                        }
                    }

                    scope.Complete();
                }

                return objSave.Id;
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.TripReason, mySession.Current.UserId);
                return 0;
            }
        }
    }
}