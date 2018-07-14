using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.EnterpriseTask
{
    public class Enterprise
    {
        Guid guid;

        public Guid Guid
        {
            get
            {
                return guid;
            }
        }

        public Enterprise(Guid guid)
        {
            this.guid = guid;
        }

        string name;

        public string Name
        {
            get;set;
        }

        public void SetName(string name) { this.name = name; }    

        string inn;

        public string Inn
        {
            get
            {
                return inn;
            }

            set
            {
                if (inn.Length != 10 || !inn.All(z => char.IsDigit(z)))
                    throw new ArgumentException();
                
            }
        }

        DateTime establishDate;

        public DateTime EstablishDate
        {
            get;set;
            
        }

        public void SetEstablishDate(DateTime establishDate)
        {
            this.establishDate = establishDate;
        }

        public TimeSpan ActiveTimeSpan
        {
            get
            {
                return DateTime.Now - establishDate;
            }
        }

        public double GetTotalTransactionsAmount()
        {
            DataBase.OpenConnection();
            var amount = 0.0;
            foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == guid))
                amount += t.Amount;
            return amount;
        }
    }
}