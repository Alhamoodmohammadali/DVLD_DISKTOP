using System;
using System.Data;
using System.Xml.Linq;
using DVLD_DataAccess;
using static DVLD_DataAccess.clsPersonData;
namespace DVLD_Buisness
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int? PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName { get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; } }
        public string NationalNo { set; get; }
        public DateTime? DateOfBirth { set; get; }
        public short Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int? NationalityCountryID { set; get; }
        private string _ImagePath;

        public clsCountry CountryInfo;
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        public clsPerson()
        {
            this.PersonID = null;
            this.FirstName = null;
            this.SecondName = null;
            this.ThirdName = null;
            this.LastName = null;
            this.DateOfBirth = null;
            this.Address = null;
            this.Phone = null;
            this.Email = null;
            this.NationalityCountryID = null;
            this.ImagePath = null;
            Mode = enMode.AddNew;
        }
        private clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,string LastName, string NationalNo, DateTime DateOfBirth, short Gendor,string Address, string Phone, string Email,int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;
        }
        private bool _AddNewPerson()=> ((this.PersonID = AddNewPerson(this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.NationalNo, (DateTime)this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, (int)this.NationalityCountryID, this.ImagePath)).HasValue);
        
        private bool _UpdatePerson() => UpdatePerson((int)this.PersonID, this.FirstName, this.SecondName, this.ThirdName,this.LastName, this.NationalNo,(DateTime) this.DateOfBirth, this.Gendor,this.Address, this.Phone, this.Email,(int)this.NationalityCountryID, this.ImagePath);
        public static clsPerson Find(int PersonID)
        {
            string FirstName =null, SecondName = null, ThirdName = null, LastName = null, NationalNo = null, Email = null, Phone = null, Address = null, ImagePath = null;
            DateTime? DateOfBirth = null;
            int? NationalityCountryID = null;
            short? Gendor =null;
            if (GetPersonInfoByID(PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref NationalNo, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,NationalNo, (DateTime) DateOfBirth, (short)Gendor, Address, Phone, Email, (int)NationalityCountryID, ImagePath);
            else
                return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName =null, SecondName = null, ThirdName = null, LastName = null, Email  = null, Phone = null, Address = null, ImagePath = null;
            DateTime? DateOfBirth = null;
            int? PersonID = null, NationalityCountryID = null;
            short? Gendor = null;
            if (GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
                return new clsPerson((int)PersonID, FirstName, SecondName, ThirdName, LastName,NationalNo,(DateTime) DateOfBirth,(short) Gendor, Address, Phone, Email,(int) NationalityCountryID, ImagePath);
            else
                return null;
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }
        public static DataTable GetAllPeople() => _GetAllPeople();
        public static bool DeletePerson(int ID) => _DeletePerson(ID);
        public static bool isPersonExist(int ID) => IsPersonExist(ID);
        public static bool isPersonExist(string NationalNo) => IsPersonExist(NationalNo);
        
    }
}
