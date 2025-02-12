import { useState } from "react";
import httpClient from "../Api/HttpClient";
import "bootstrap/dist/css/bootstrap.css";
import { useParams } from "react-router-dom";
import { useEffect } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { ready } from "jquery";
import { useNavigate } from "react-router-dom";

export default function AddEmployee() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phoneNo, setPhoneNo] = useState(0);
  const [roles, setRoles] = useState([]);
  const [street, setStreet] = useState("");
  const [city, setCity] = useState("");
  const [selectedRole, setSelectedRole] = useState(null);
  const [postalCode, setPostalCode] = useState("");
  const [employee, setEmployee] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const { id } = useParams();

  useEffect(() => {
    async function fetchStatus() {
      try {
        var response = await httpClient.get(
          "https://localhost:7058/api/Enum/getRoles"
        );
        console.log(response);
        setRoles(response.data);
      } catch (err) {
        console.log(err);
        toast.error("Roles: could not fetch roles.");
      }
    }
    fetchStatus();
  }, []);

  const fetchEmployee = async () => {
    try {
      const response = await httpClient
        .get(`/api/Employee/getEmployeeByGuidId?guidId=${id}`)
        .finally(setLoading(false));
      setName(response.data.name);
      setEmail(response.data.email);
      setCity(response.data.address.city);
      setPhoneNo(response.data.phoneNumber);
      setSelectedRole(response.data.userRoleId);
      setPostalCode(response.data.address.postalCode);
      setStreet(response.data.address.street);

      setEmployee(response.data); // Update the employee state with response data
      //   setRequests(response.data.request);
      console.log(response);
    } catch (err) {
      toast.error("Couldn't fetch employee!!!");
      console.log(err.message);
      console.log(err.response.data.errors.guidId[0]);
    }
  };

  useEffect(() => {
    fetchEmployee();
  }, []);

  const handleUpdateEmployee = async (e) => {
    try {
      console.log(selectedRole);
      console.log(id);
      e.preventDefault();
      const response = await httpClient.put("/api/Employee/updateEmployee", {
        id: id,
        name: name,
        email: email,
        userRoleId: selectedRole,
        phoneNumber: phoneNo,
        address: {
          street: street,
          city: city,
          postalCode: postalCode,
        },
      });
      console.log(response.data);
      navigate("/", {
        state: { showToast: "Employee updated successfully!!!" },
      });
    } catch (err) {
      console.log(err);
      if (err.response && Array.isArray(err.response.data.errors)) {
        err.response.data.errors.forEach((errorObj) => {
          toast.error(`${errorObj.fieldName}: ${errorObj.descriptions}`);
        });
      } else {
        toast.error("something went wrong!!!");
      }
    }
  };
  return (
    <section className="vh-100 gradient-custom">
      <div className="container py-5">
        {/* <ToastContainer /> */}
        <div className="row  d-flex justify-content-center h-100">
          <div className="col-10">
            <div
              className="card bg-light text-dark"
              style={{ borderRadius: "1rem" }}
            >
              <div className="mt-4 mx-5 text-center">
                <h1>Edit Employee</h1>
              </div>
              {/* ------------------------------------------------------------------------- */}
              {/* <div className="row mb-5 d-flex justify-content-center ">
                <div
                  className="card bg-light col-2 h-50 text-dark mt-5"
                  style={{ borderRadius: "1rem" }}
                > */}
              <div className="container">
                <ToastContainer />
                <div className="row justify-content-center">
                  <div className="col-md-6 mt-3">
                    <form>
                      <div className="form-row">
                        <div className="row d-flex justify-content-center">
                          <div className="form-group col-md-5 mb-4">
                            <label htmlFor="inputEmail4">Name</label>
                            <input
                              type="text"
                              value={name}
                              onChange={(e) => setName(e.target.value)}
                              className="form-control"
                              required
                            />
                          </div>
                          <div className="form-group col-md-5 mb-4">
                            <label htmlFor="inputEmail4">Email</label>
                            <input
                              type="email"
                              value={email}
                              onChange={(e) => setEmail(e.target.value)}
                              className="form-control"
                              required
                              disabled
                            />
                          </div>
                        </div>
                        <div className="row d-flex justify-content-center">
                          <div className="form-group col-md-5 mb-4">
                            <label htmlFor="dropdown">Select a Role:</label>
                            <select
                              className="form-control"
                              id="dropdown"
                              value={selectedRole}
                              onChange={(e) => setSelectedRole(e.target.value)}
                            >
                              <option value="">Select an option</option>
                              {roles.map((role) => (
                                <option key={role.id} value={role.id}>
                                  {role.name}
                                </option>
                              ))}
                            </select>
                          </div>
                          <div className="form-group col-md-5 mb-4">
                            <label htmlFor="inputEmail4">Phone number</label>
                            <input
                              type="text"
                              value={phoneNo}
                              required
                              onChange={(e) => setPhoneNo(e.target.value)}
                              className="form-control"
                            />
                          </div>
                        </div>
                      </div>

                      <div className="row d-flex justify-content-center mb-3">
                        <div className="col-md-4 mb-4">
                          <label htmlFor="inputAddress">Street</label>
                          <input
                            type="text"
                            value={street}
                            onChange={(e) => setStreet(e.target.value)}
                            className="form-control"
                            required
                          />
                        </div>
                        <div className="col-md-3 mb-4">
                          <label htmlFor="inputCity">City</label>
                          <input
                            type="text"
                            value={city}
                            onChange={(e) => setCity(e.target.value)}
                            className="form-control"
                            required
                          />
                        </div>
                        <div className="col-md-3">
                          <label htmlFor="inputZip">PostalCode</label>
                          <input
                            type="text"
                            value={postalCode}
                            onChange={(e) => setPostalCode(e.target.value)}
                            className="form-control"
                            required
                          />
                        </div>
                      </div>
                      <hr />
                      <div className="d-flex justify-content-evenly mb-3">
                        <button
                          type="submit"
                          id="button"
                          onClick={handleUpdateEmployee}
                          className="btn btn-primary my-3  ml-5"
                        >
                          Update Employee
                        </button>
                        <button
                          onClick={() => navigate("/")}
                          className="btn btn-secondary my-3 mx-4"
                        >
                          Back
                        </button>
                      </div>
                    </form>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      {/* </div>
      </div> */}
    </section>
  );
}
