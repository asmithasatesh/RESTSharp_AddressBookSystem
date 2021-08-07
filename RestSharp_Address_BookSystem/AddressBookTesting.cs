using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace RestSharp_Address_BookSystem
{
    [TestClass]
    public class AddressBookTesting
    {
        //Initializing the restclient as null
        RestClient client = null;
        [TestInitialize]
        //Setup base 
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000");
        }

        public IRestResponse GetAllContacts()
        {
            //Define method Type
            RestRequest request = new RestRequest("/AddressBookContacts", Method.GET);
            //Eexcute request
            IRestResponse response = client.Execute(request);
            //Return the response
            return response;
        }
        //Usecase 1: Getting all the employee details from json server
        [TestMethod]
        public void OnCallingGetAPI_ReturnContacts()
        {
            IRestResponse response = GetAllContacts();

            //Deserialize json object to List
            var jsonObject = JsonConvert.DeserializeObject<List<ContactModel>>(response.Content);
            Assert.AreEqual(2,jsonObject.Count);
            foreach (var element in jsonObject)
            {
                Console.WriteLine("FirstName: {0} || LastName: {1} || Address :{2} || City: {3} || State: {4} || Zip: {5} || PhoneNumber: {6} || Email: {7} ", element.firstName, element.lastName, element.Address, element.City, element.State, element.Zip, element.PhoneNumber, element.Email);
            }

            //Check the status code 
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //add data to json server
        public IRestResponse AddingInJsonServer(JsonObject jsonObject)
        {
            RestRequest request = new RestRequest("/AddressBookContacts", Method.POST);
            request.AddParameter("application/json", jsonObject, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response;

        }
        //Usecase 3: 
        [TestMethod]
        public void OnCallingPostAPI_Adding_MultipleData()
        {
            //Create Json object for employee one
            JsonObject employeeOne = new JsonObject();
            employeeOne.Add("firstName", "Dhana");
            employeeOne.Add("lastName", "Lakshmi");
            employeeOne.Add("Address", "131 martha street");
            employeeOne.Add("City", "Pune");
            employeeOne.Add("State", "Maharashtra");
            employeeOne.Add("Zip", 243000);
            employeeOne.Add("PhoneNumber", 900054540);
            employeeOne.Add("Email", "Dhana@gmail.com");
            //Call Function to Add
            HttpStatusCode responseOne = AddingInJsonServer(employeeOne).StatusCode;

            //Create Json object for employee Two
            JsonObject employeeTwo = new JsonObject();
            employeeTwo.Add("firstName", "Maruthi");
            employeeTwo.Add("lastName", "dev");
            employeeTwo.Add("Address", "Bakers street");
            employeeTwo.Add("City", "Lucknow");
            employeeTwo.Add("State", "UP");
            employeeTwo.Add("Zip", 630116);
            employeeTwo.Add("PhoneNumber", 920054540);
            employeeTwo.Add("Email", "maruti@gmail.com");
            //Call Function to Add
            HttpStatusCode responseTwo = AddingInJsonServer(employeeTwo).StatusCode;

            Assert.AreEqual(responseOne, HttpStatusCode.Created);
            Assert.AreEqual(responseTwo, HttpStatusCode.Created);
        }
        //Usecase 4: Method to update values for given ID
        [TestMethod]
        public void GivenPUTAPI_retrunResponse()
        {
            RestRequest request = new RestRequest("/AddressBookContacts/4", Method.PUT);
            JsonObject employeeTwo = new JsonObject();
            employeeTwo.Add("firstName", "Maruthi");
            employeeTwo.Add("lastName", "dev");
            employeeTwo.Add("Address", "BI BAZAR");
            employeeTwo.Add("City", "Lucknow");
            employeeTwo.Add("State", "UP");
            employeeTwo.Add("Zip", 630116);
            employeeTwo.Add("PhoneNumber", 920054540);
            employeeTwo.Add("Email", "maruti@gmail.com");
            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<ContactModel>(response.Content);
            Console.WriteLine("FirstName: {0} || LastName: {1} || Address :{2} || City: {3} || State: {4} || Zip: {5} || PhoneNumber: {6} || Email: {7} || contactid: {8}", result.firstName, result.lastName, result.Address, result.City, result.State, result.Zip, result.PhoneNumber, result.Email, result.id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }
        // Usecase 5: Delete the employee details using the id
        [TestMethod]
        public void OnCallingDeleteAPI_DeleteEmployeeDetails()
        {
            //Passing the method type as put(update existing employee details)

            RestRequest request = new RestRequest("/AddressBookContacts/3", Method.DELETE);
            IRestResponse response = client.Execute(request);
            //check count after deletion
            IRestResponse getresponse = GetAllContacts();
            List<ContactModel> resultList = JsonConvert.DeserializeObject<List<ContactModel>>(getresponse.Content);
            foreach (var result in resultList)
            {
                Console.WriteLine("FirstName: {0} || LastName: {1} || Address :{2} || City: {3} || State: {4} || Zip: {5} || PhoneNumber: {6} || Email: {7} || contactid: {8}", result.firstName, result.lastName, result.Address, result.City, result.State, result.Zip, result.PhoneNumber, result.Email, result.id);
            }
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
