using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_Charge_Customer_Count()
        {
            //arrange
            ICheckInFee stubCheckInFee = MockRepository.GenerateStub<ICheckInFee>();
            Pub target = new Pub(stubCheckInFee);

            stubCheckInFee.Stub(x => x.GetFee(Arg<Customer>.Is.Anything)).Return(100);

            var customers = new List<Customer>
            {
                new Customer {IsMale = true},
                new Customer {IsMale = false},
                new Customer {IsMale = false},
            };

            decimal expected = 1;

            //act
            var actual = target.CheckIn(customers);

            //assert
            Assert.AreEqual(expected, actual);
        }

        public interface ICheckInFee
        {
            decimal GetFee(Customer customer);
        }

        public class Customer
        {
            public bool IsMale { get; set; }

            public int Seq { get; set; }
        }

        public class Pub
        {
            private ICheckInFee _checkInFee;
            private decimal _inCome = 0;

            public Pub(ICheckInFee checkInFee)
            {
                this._checkInFee = checkInFee;
            }

            /// <summary>
            /// �J��
            /// </summary>
            /// <param name="customers"></param>
            /// <returns>���O���H��</returns>
            public int CheckIn(List<Customer> customers)
            {
                var result = 0;

                foreach (var customer in customers)
                {
                    var isFemale = !customer.IsMale;

                    //�k�ͧK�O�J��
                    if (isFemale)
                    {
                        continue;
                    }
                    else
                    {
                        //for stub, validate status: income value
                        //for mock, validate only male
                        this._inCome += this._checkInFee.GetFee(customer);

                        result++;
                    }
                }

                //for stub, validate return value
                return result;
            }

            public decimal GetInCome()
            {
                return this._inCome;
            }
        }
    }
}