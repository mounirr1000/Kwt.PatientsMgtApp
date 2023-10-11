using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwt.PatientsMgtApp.Core.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

       // [Required]
        [DisplayName("Employee ID")]
        //[MaxLength(50, ErrorMessage = "Agency Name should not be more than 50 characters")]
        public int EmployeeID { get; set; }

        [Required]
        [DisplayName("First Name")]
        
        public string EmployeeFName { get; set; }
       
        [DisplayName("Midlle Name")]
        
        public string EmployeeMName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        
        public string EmployeeLName { get; set; }

        public string EmployeeName { get { return EmployeeFName + " " + EmployeeMName + " " + EmployeeLName; } }
        [DisplayName("Phone Number")]
        
        public string Employeephone { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        
        [DisplayName("Email")]
        
        public string Email { get; set; }
        [DisplayName("Street Address")]        
        public string StreetAddress { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Zip code")]
        public string ZipCode { get; set; }

        public string Address { get { return StreetAddress+" "+ City + ", "+ State + " "+ ZipCode + " "; } }
        [DisplayName("Gender")]
        public string GENDER { get; set; }

        [DisplayName("Social Status Name")]
        public string SocialStatus { get; set; }

        [DisplayName("Immigration status")]
        public string ImmigrationStatus { get; set; }

        [DisplayName("Nationality")]
        public string Nationality { get; set; }

        [DisplayName("Education")]
        public string Education { get; set; }

        [DisplayName("Training")]
        public string Training { get; set; }

        [DisplayName("Qualification")]
        public string Qualification { get; set; }

        [DisplayName("Hire Date")]
        public Nullable<System.DateTime> HireDate { get; set; }

        [DisplayName("End Date")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [DisplayName("Birth Date")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [DisplayName("Notes")]
        public string Notes { get; set; }
        public string Photograph { get; set; }

        public string CreatedBy { get; set; }

        [DisplayName("Marital Status")]
        public int SocialStatusID { get; set; }

        public decimal? TotalSalary { get; set; }
        public SalaryModel Salary { get; set; }
        public BonusModel Bonus { get; set; }
        
        public OvertimeModel Overtime { get; set; }
        public EmployeeAccountTypeModel Account { get; set; }
        public SalaryDeductionModel Deduction { get; set; }
        public TitleModel Title { get; set; }

        public List<TitleTypesModel> TitleTypes { get; set; }

        [DisplayName("Tax Category")]
        public List<TaxCategoryModel> TaxCategories { get; set; }
        [DisplayName("Bonus Type")]

        public List<BonusTypeModel> BonusTypes { get; set; }

        [DisplayName("Insurance Option")]
        public List<InsuranceOptionModel> InsuranceOptions { get; set; }

        [DisplayName("Employee Insurance Number")]
        public List<EmployeeInsuranceModel> EmployeeInsurances { get; set; }

        [DisplayName("Insurance Type")]
        public List<InsuranceTypeModel> InsuranceTypes { get; set; }
        [DisplayName("Payroll Accounts")]
        public List<PayrollAccountModel> PayrollAccounts { get; set; }

        public List<SocialStatusModel> SocialStatuses { get; set; }
        public EmployeeInsuranceModel Insurance { get; set; }
        public EmployeeInsuranceModel HealthInsurance { get; set; }
        public EmployeeInsuranceModel DentalInsurance { get; set; }
        public EmployeeInsuranceModel OtherInsurance { get; set; }
        public List<AccountModel> Accounts { get; set; }
        public List<Char> Genders { get { return new List<char>() { 'M','F'}; } }
        public List<string> Nationalties { get { return new List<string>() { "Afghan"
                                                                        , "Albanian"
                                                                        , "Algerian"
                                                                        , "American"
                                                                        , "Andorran"
                                                                        , "Angolan"
                                                                        , "Antiguans"
                                                                        , "Argentinean"
                                                                        , "Armenian"
                                                                        , "Australian"
                                                                        , "Austrian"
                                                                        , "Azerbaijani"
                                                                        , "Bahamian"
                                                                        , "Bahraini"
                                                                        , "Bangladeshi"
                                                                        , "Barbadian"
                                                                        , "Barbudans"
                                                                        , "Batswana"
                                                                        , "Belarusian"
                                                                        , "Belgian"
                                                                        , "Belizean"
                                                                        , "Beninese"
                                                                        , "Bhutanese"
                                                                        , "Bolivian"
                                                                        , "Bosnian"
                                                                        , "Brazilian"
                                                                        , "British"
                                                                        , "Bruneian"
                                                                        , "Bulgarian"
                                                                        , "Burkinabe"
                                                                        , "Burmese"
                                                                        , "Burundian"
                                                                        , "Cambodian"
                                                                        , "Cameroonian"
                                                                        , "Canadian"
                                                                        , "Cape Verdean"
                                                                        , "Central African"
                                                                        , "Chadian"
                                                                        , "Chilean"
                                                                        , "Chinese"
                                                                        , "Colombian"
                                                                        , "Comoran"
                                                                        , "Congolese"
                                                                        , "Costa Rican"
                                                                        , "Croatian"
                                                                        , "Cuban"
                                                                        , "Cypriot"
                                                                        , "Czech"
                                                                        , "Danish"
                                                                        , "Djibouti"
                                                                        , "Dominican"
                                                                        , "Dutch"
                                                                        , "East Timorese"
                                                                        , "Ecuadorean"
                                                                        , "Egyptian"
                                                                        , "Emirian"
                                                                        , "Equatorial Guinean"
                                                                        , "Eritrean"
                                                                        , "Estonian"
                                                                        , "Ethiopian"
                                                                        , "Fijian"
                                                                        , "Filipino"
                                                                        , "Finnish"
                                                                        , "French"
                                                                        , "Gabonese"
                                                                        , "Gambian"
                                                                        , "Georgian"
                                                                        , "German"
                                                                        , "Ghanaian"
                                                                        , "Greek"
                                                                        , "Grenadian"
                                                                        , "Guatemalan"
                                                                        , "Guinea,Bissauan"
                                                                        , "Guinean"
                                                                        , "Guyanese"
                                                                        , "Haitian"
                                                                        , "Herzegovinian"
                                                                        , "Honduran"
                                                                        , "Hungarian"
                                                                        , "I,Kiribati"
                                                                        , "Icelander"
                                                                        , "Indian"
                                                                        , "Indonesian"
                                                                        , "Iranian"
                                                                        , "Iraqi"
                                                                        , "Irish"
                                                                        , "Israeli"
                                                                        , "Italian"
                                                                        , "Ivorian"
                                                                        , "Jamaican"
                                                                        , "Japanese"
                                                                        , "Jordanian"
                                                                        , "Kazakhstani"
                                                                        , "Kenyan"
                                                                        , "Kittian and Nevisian"
                                                                        , "Kuwaiti"
                                                                        , "Kyrgyz"
                                                                        , "Laotian"
                                                                        , "Latvian"
                                                                        , "Lebanese"
                                                                        , "Liberian"
                                                                        , "Libyan"
                                                                        , "Liechtensteiner"
                                                                        , "Lithuanian"
                                                                        , "Luxembourger"
                                                                        , "Macedonian"
                                                                        , "Malagasy"
                                                                        , "Malawian"
                                                                        , "Malaysian"
                                                                        , "Maldivian"
                                                                        , "Malian"
                                                                        , "Maltese"
                                                                        , "Marshallese"
                                                                        , "Mauritanian"
                                                                        , "Mauritian"
                                                                        , "Mexican"
                                                                        , "Micronesian"
                                                                        , "Moldovan"
                                                                        , "Monacan"
                                                                        , "Mongolian"
                                                                        , "Moroccan"
                                                                        , "Mosotho"
                                                                        , "Motswana"
                                                                        , "Mozambican"
                                                                        , "Namibian"
                                                                        , "Nauruan"
                                                                        , "Nepalese"
                                                                        , "New Zealander"
                                                                        , "Ni,Vanuatu"
                                                                        , "Nicaraguan"
                                                                        , "Nigerian"
                                                                        , "Nigerien"
                                                                        , "North Korean"
                                                                        , "Northern Irish"
                                                                        , "Norwegian"
                                                                        , "Omani"
                                                                        , "Pakistani"
                                                                        , "Palauan"
                                                                        , "Panamanian"
                                                                        , "Papua New Guinean"
                                                                        , "Paraguayan"
                                                                        , "Peruvian"
                                                                        , "Polish"
                                                                        , "Portuguese"
                                                                        , "Qatari"
                                                                        , "Romanian"
                                                                        , "Russian"
                                                                        , "Rwandan"
                                                                        , "Saint Lucian"
                                                                        , "Salvadoran"
                                                                        , "Samoan"
                                                                        , "San Marinese"
                                                                        , "Sao Tomean"
                                                                        , "Saudi"
                                                                        , "Scottish"
                                                                        , "Senegalese"
                                                                        , "Serbian"
                                                                        , "Seychellois"
                                                                        , "Sierra Leonean"
                                                                        , "Singaporean"
                                                                        , "Slovakian"
                                                                        , "Slovenian"
                                                                        , "Solomon Islander"
                                                                        , "Somali"
                                                                        , "South African"
                                                                        , "South Korean"
                                                                        , "Spanish"
                                                                        , "Sri Lankan"
                                                                        , "Sudanese"
                                                                        , "Surinamer"
                                                                        , "Swazi"
                                                                        , "Swedish"
                                                                        , "Swiss"
                                                                        , "Syrian"
                                                                        , "Taiwanese"
                                                                        , "Tajik"
                                                                        , "Tanzanian"
                                                                        , "Thai"
                                                                        , "Togolese"
                                                                        , "Tongan"
                                                                        , "Trinidadian or Tobagonian"
                                                                        , "Tunisian"
                                                                        , "Turkish"
                                                                        , "Tuvaluan"
                                                                        , "Ugandan"
                                                                        , "Ukrainian"
                                                                        , "Uruguayan"
                                                                        , "Uzbekistani"
                                                                        , "Venezuelan"
                                                                        , "Vietnamese"
                                                                        , "Welsh"
                                                                        , "Yemenite"
                                                                        , "Zambian"
                                                                        , "Zimbabwean", }; }  }
        public List<string> Countries { get { return new List<string>() {"Afghanistan",
                                                                            "Albania",
                                                                            "Algeria",
                                                                            "American Samoa",
                                                                            "Andorra",
                                                                            "Angola",
                                                                            "Anguilla",
                                                                            "Antarctica",
                                                                            "Antigua and Barbuda",
                                                                            "Argentina",
                                                                            "Armenia",
                                                                            "Aruba",
                                                                            "Australia",
                                                                            "Austria",
                                                                            "Azerbaijan",
                                                                            "Bahamas",
                                                                            "Bahrain",
                                                                            "Bangladesh",
                                                                            "Barbados",
                                                                            "Belarus",
                                                                            "Belgium",
                                                                            "Belize",
                                                                            "Benin",
                                                                            "Bermuda",
                                                                            "Bhutan",
                                                                            "Bolivia",
                                                                            "Bosnia and Herzegovina",
                                                                            "Botswana",
                                                                            "Bouvet Island",
                                                                            "Brazil",
                                                                            "British Indian Ocean Territory",
                                                                            "Brunei Darussalam",
                                                                            "Bulgaria",
                                                                            "Burkina Faso",
                                                                            "Burundi",
                                                                            "Cambodia",
                                                                            "Cameroon",
                                                                            "Canada",
                                                                            "Cape Verde",
                                                                            "Cayman Islands",
                                                                            "Central African Republic",
                                                                            "Chad",
                                                                            "Chile",
                                                                            "China",
                                                                            "Christmas Island",
                                                                            "Cocos (Keeling) Islands",
                                                                            "Colombia",
                                                                            "Comoros",
                                                                            "Congo",
                                                                            "Congo, the Democratic Republic of the",
                                                                            "Cook Islands",
                                                                            "Costa Rica",
                                                                            "Cote D'Ivoire",
                                                                            "Croatia",
                                                                            "Cuba",
                                                                            "Cyprus",
                                                                            "Czech Republic",
                                                                            "Denmark",
                                                                            "Djibouti",
                                                                            "Dominica",
                                                                            "Dominican Republic",
                                                                            "Ecuador",
                                                                            "Egypt",
                                                                            "El Salvador",
                                                                            "Equatorial Guinea",
                                                                            "Eritrea",
                                                                            "Estonia",
                                                                            "Ethiopia",
                                                                            "Falkland Islands (Malvinas)",
                                                                            "Faroe Islands",
                                                                            "Fiji",
                                                                            "Finland",
                                                                            "France",
                                                                            "French Guiana",
                                                                            "French Polynesia",
                                                                            "French Southern Territories",
                                                                            "Gabon",
                                                                            "Gambia",
                                                                            "Georgia",
                                                                            "Germany",
                                                                            "Ghana",
                                                                            "Gibraltar",
                                                                            "Greece",
                                                                            "Greenland",
                                                                            "Grenada",
                                                                            "Guadeloupe",
                                                                            "Guam",
                                                                            "Guatemala",
                                                                            "Guinea",
                                                                            "Guinea-Bissau",
                                                                            "Guyana",
                                                                            "Haiti",
                                                                            "Heard Island and Mcdonald Islands",
                                                                            "Holy See (Vatican City State)",
                                                                            "Honduras",
                                                                            "Hong Kong",
                                                                            "Hungary",
                                                                            "Iceland",
                                                                            "India",
                                                                            "Indonesia",
                                                                            "Iran, Islamic Republic of",
                                                                            "Iraq",
                                                                            "Ireland",
                                                                            "Israel",
                                                                            "Italy",
                                                                            "Jamaica",
                                                                            "Japan",
                                                                            "Jordan",
                                                                            "Kazakhstan",
                                                                            "Kenya",
                                                                            "Kiribati",
                                                                            "Korea, Democratic People's Republic of",
                                                                            "Korea, Republic of",
                                                                            "Kuwait",
                                                                            "Kyrgyzstan",
                                                                            "Lao People's Democratic Republic",
                                                                            "Latvia",
                                                                            "Lebanon",
                                                                            "Lesotho",
                                                                            "Liberia",
                                                                            "Libyan Arab Jamahiriya",
                                                                            "Liechtenstein",
                                                                            "Lithuania",
                                                                            "Luxembourg",
                                                                            "Macao",
                                                                            "Macedonia, the Former Yugoslav Republic of",
                                                                            "Madagascar",
                                                                            "Malawi",
                                                                            "Malaysia",
                                                                            "Maldives",
                                                                            "Mali",
                                                                            "Malta",
                                                                            "Marshall Islands",
                                                                            "Martinique",
                                                                            "Mauritania",
                                                                            "Mauritius",
                                                                            "Mayotte",
                                                                            "Mexico",
                                                                            "Micronesia, Federated States of",
                                                                            "Moldova, Republic of",
                                                                            "Monaco",
                                                                            "Mongolia",
                                                                            "Montserrat",
                                                                            "Morocco",
                                                                            "Mozambique",
                                                                            "Myanmar",
                                                                            "Namibia",
                                                                            "Nauru",
                                                                            "Nepal",
                                                                            "Netherlands",
                                                                            "Netherlands Antilles",
                                                                            "New Caledonia",
                                                                            "New Zealand",
                                                                            "Nicaragua",
                                                                            "Niger",
                                                                            "Nigeria",
                                                                            "Niue",
                                                                            "Norfolk Island",
                                                                            "Northern Mariana Islands",
                                                                            "Norway",
                                                                            "Oman",
                                                                            "Pakistan",
                                                                            "Palau",
                                                                            "Palestinian Territory, Occupied",
                                                                            "Panama",
                                                                            "Papua New Guinea",
                                                                            "Paraguay",
                                                                            "Peru",
                                                                            "Philippines",
                                                                            "Pitcairn",
                                                                            "Poland",
                                                                            "Portugal",
                                                                            "Puerto Rico",
                                                                            "Qatar",
                                                                            "Reunion",
                                                                            "Romania",
                                                                            "Russian Federation",
                                                                            "Rwanda",
                                                                            "Saint Helena",
                                                                            "Saint Kitts and Nevis",
                                                                            "Saint Lucia",
                                                                            "Saint Pierre and Miquelon",
                                                                            "Saint Vincent and the Grenadines",
                                                                            "Samoa",
                                                                            "San Marino",
                                                                            "Sao Tome and Principe",
                                                                            "Saudi Arabia",
                                                                            "Senegal",
                                                                            "Serbia and Montenegro",
                                                                            "Seychelles",
                                                                            "Sierra Leone",
                                                                            "Singapore",
                                                                            "Slovakia",
                                                                            "Slovenia",
                                                                            "Solomon Islands",
                                                                            "Somalia",
                                                                            "South Africa",
                                                                            "South Georgia and the South Sandwich Islands",
                                                                            "Spain",
                                                                            "Sri Lanka",
                                                                            "Sudan",
                                                                            "Suriname",
                                                                            "Svalbard and Jan Mayen",
                                                                            "Swaziland",
                                                                            "Sweden",
                                                                            "Switzerland",
                                                                            "Syrian Arab Republic",
                                                                            "Taiwan, Province of China",
                                                                            "Tajikistan",
                                                                            "Tanzania, United Republic of",
                                                                            "Thailand",
                                                                            "Timor-Leste",
                                                                            "Togo",
                                                                            "Tokelau",
                                                                            "Tonga",
                                                                            "Trinidad and Tobago",
                                                                            "Tunisia",
                                                                            "Turkey",
                                                                            "Turkmenistan",
                                                                            "Turks and Caicos Islands",
                                                                            "Tuvalu",
                                                                            "Uganda",
                                                                            "Ukraine",
                                                                            "Uknown",
                                                                            "United Arab Emirates",
                                                                            "United Kingdom",
                                                                            "United States",
                                                                            "United States Minor Outlying Islands",
                                                                            "Uruguay",
                                                                            "Uzbekistan",
                                                                            "Vanuatu",
                                                                            "Venezuela",
                                                                            "Viet Nam",
                                                                            "Virgin Islands, British",
                                                                            "Virgin Islands, US",
                                                                            "Wallis and Futuna",
                                                                            "Western Sahara",
                                                                            "Yemen",
                                                                            "Zambia",
                                                                            "Zimbabwe", }; }  }
        public List<string> ImmigrationStatuses { get { return new List<string>() { "U visas", "T visas", "Student visas", "Visitor visas", "Temporary worker visas", "US citizen", "Legal Permanent Resident",
                            "Conditional Permanent Resident",
                             "Asylee or Refugee",
                            "Person with Temporary Protected Status",
                            "Undocumented person",
                            "Other"}; } }
        public List<string> EductaionList { get { return new List<string>() { "Undergraduate Degree", "Associate's Degree", "Bachelor's Degree", "Graduate Degree", "Master's Degree", "Doctoral Degree", "High School Degree", "GED", "Professional Degree", "Specialist Degree","High School","Middle School", "Primary School", "College","University","No School","Other" }; } }
        public List<string> States { get { return new List<string>() {
                        "AK",
                      "AL",
                      "AR",
                      "AS",
                      "AZ",
                      "CA",
                      "CO",
                      "CT",
                      "DC",
                      "DE",
                      "FL",
                      "GA",
                      "GU",
                      "HI",
                      "IA",
                      "ID",
                      "IL",
                      "IN",
                      "KS",
                      "KY",
                      "LA",
                      "MA",
                      "MD",
                      "ME",
                      "MI",
                      "MN",
                      "MO",
                      "MS",
                      "MT",
                      "NC",
                      "ND",
                      "NE",
                      "NH",
                      "NJ",
                      "NM",
                      "NV",
                      "NY",
                      "OH",
                      "OK",
                      "OR",
                      "PA",
                      "PR",
                      "RI",
                      "SC",
                      "SD",
                      "TN",
                      "TX",
                      "UT",
                      "VA",
                      "VI",
                      "VT",
                      "WA",
                      "WI",
                      "WV",
                      "WY"
        }; } }

       
    }
}
