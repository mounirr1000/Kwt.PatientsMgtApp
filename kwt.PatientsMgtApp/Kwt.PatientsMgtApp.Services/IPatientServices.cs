using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;
using Kwt.PatientsMgtApp.Core.Models;

namespace Kwt.PatientsMgtApp.Services
{
    [ServiceContract]
    public interface IPatientServices
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<PatientModel> GetPatients();
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/patient/{patientcid}/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PatientModel GetPatient(string patientcid);
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/patient/{patientcid}/companions/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        List<CompanionModel> GetPatientCompanions(string patientcid);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/patient/new", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        void AddPatient(PatientModel patient);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/patient/update/", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        PatientModel UpdatePatient(PatientModel patient);
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/patient/delete", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        int DeletePatient(PatientModel patient);
    }
}
