import { useCallback, useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.css";
import httpClient from "../Api/HttpClient";
import Request from "../Request/Request";
import { useParams } from "react-router-dom";
import AuthContext from "../../context/AuthProvider";
import useAuth from "../hooks/useAuth";

export default function EmployeeDetails() {
  const [employee, setEmployee] = useState(null);
  const [loading, setLoading] = useState(true);
  const [requests, setRequests] = useState([]);
  const { id } = useParams();

  const { auth } = useAuth(AuthContext);

  const fetchEmployee = async () => {
    let userId;

    if (id === undefined) {
      console.log(auth);
      userId = auth.guidId;
    } else {
      userId = id;
    }

    try {
      const response = await httpClient
        .get(`/api/Employee/getEmployeeByGuidId?guidId=${userId}`)
        .finally(setLoading(false));
      // const request = await httpClient
      //   .get(`/api/Request/getAllRequestByUserGuidId?guidId=${userId}`)
      //   .finally(setLoading(false));

      setEmployee(response.data); // Update the employee state with response data
      setRequests(response.data.requests);
      console.log(response);
    } catch (err) {
      console.log(err);
      console.log(err.message);
      // console.log(err.response.data.errors.guidId[0]);
    }
  };

  useEffect(() => {
    fetchEmployee();
  }, []);

  return (
    <div>
      {!loading && employee ? (
        <div className="container">
          <div className="row mb-5">
            <div className="col-md-5"></div>

            <div className="col-md-4">
              <h1>{employee.name} Details</h1>
              <p>Name: {employee.name}</p>
              <p>Email: {employee.email}</p>
              <p>UserRole: {employee.userRole}</p>
              <p>PhoneNumber: {employee.phoneNumber}</p>
              <p>Street: {employee.address.street}</p>
              <p>City: {employee.address.city}</p>
              <p>PostalCode: {employee.address.postalCode}</p>
              <p>AddedOn: {employee.addedOn}</p>
              <p>AddedBy: {employee.addedByEmail}</p>
              <p>UpdatedOn: {employee.updatedOn}</p>
              <p>UpdatedBy: {employee.updatedByEmail}</p>
            </div>
            <div className="col-md-2"></div>
          </div>
          {((!loading && auth.userRole === "Admin") ||
            auth.userRole === "SuperAdmin" ||
            auth.userRole === "Manager" ||
            auth.userRole === "Ceo") && (
            <Request
              requests={requests}
              setRequests={setRequests}
              key={employee.request?.id}
            />
          )}
        </div>
      ) : (
        <p>Loading employee details...</p>
      )}
    </div>
  );
}
