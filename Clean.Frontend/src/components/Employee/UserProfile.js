import { useCallback, useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.css";
import httpClient from "../Api/HttpClient";
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

      setEmployee(response.data); // Update the employee state with response data
      setRequests(response.data.request);
      console.log(response);
    } catch (err) {
      console.log(err.message);
      console.log(err.response.data.errors.guidId[0]);
    }
  };

  useEffect(() => {
    fetchEmployee();
  }, []);

  return (
    <div>
      {!loading && employee ? (
        <div>
          <section className="vh-100 gradient-custom">
            <div className="container py-5">
              {/* <ToastContainer /> */}
              <div className="row  d-flex justify-content-center h-100">
                <div className="col-12">
                  <div
                    className="card bg-light text-dark"
                    style={{ borderRadius: "1rem" }}
                  >
                    <div className="mt-4 mx-5">
                      <h1>My Profile</h1>
                    </div>
                    {/* ------------------------------------------------------------------------- */}
                    <div className="row mb-5 d-flex justify-content-center ">
                      <div
                        className="card bg-light col-2 h-50 text-dark mt-5"
                        style={{ borderRadius: "1rem" }}
                      >
                        <div className="text-center mt-3 mb-2 " id="author">
                          <img
                            className="rounded-circle"
                            alt="avatar1"
                            src={`https://dummyimage.com/150&text=${employee.name}`}
                          />
                          <h3>{employee.name}</h3>
                          <small className="label label-warning">
                            {employee.userRole}
                          </small>
                        </div>
                      </div>
                      <div className="col-1"></div>
                      <div
                        className="card bg-light col-8 text-dark mt-5"
                        style={{ borderRadius: "1rem" }}
                      >
                        <nav class="navbar navbar-expand-lg bg-body-tertiary">
                          <div class="container-fluid">
                            <button
                              class="navbar-toggler"
                              type="button"
                              data-bs-toggle="collapse"
                              data-bs-target="#navbarNav"
                              aria-controls="navbarNav"
                              aria-expanded="false"
                              aria-label="Toggle navigation"
                            >
                              <span class="navbar-toggler-icon"></span>
                            </button>
                            <div
                              class="collapse navbar-collapse"
                              id="navbarNav"
                            >
                              <ul
                                class="navbar-nav"
                                style={{ cursor: "pointer" }}
                              >
                                <li class="nav-item">
                                  <a
                                    class={"nav-link active"}
                                    aria-current="page"
                                  >
                                    <strong>Details</strong>
                                  </a>
                                </li>
                              </ul>
                            </div>
                          </div>
                        </nav>
                        <hr />
                        <div id="detail" className=" mx-4">
                          <table class="table">
                            <tbody>
                              <tr>
                                <td>
                                  <strong>Name</strong>
                                </td>
                                <td>{employee.name}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>Email</strong>
                                </td>
                                <td>{employee.email}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>UserRole</strong>
                                </td>

                                <td>{employee.userRole}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>PhoneNumber</strong>
                                </td>

                                <td>{employee.phoneNumber}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>City</strong>
                                </td>

                                <td>{employee.address.city}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>Street</strong>
                                </td>

                                <td>{employee.address.street}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>PostalCode</strong>
                                </td>

                                <td>{employee.address.postalCode}</td>
                              </tr>
                              <tr>
                                <td>
                                  <strong>Manager</strong>
                                </td>

                                <td>{employee.managerEmail}</td>
                              </tr>
                            </tbody>
                          </table>
                        </div>
                        {/* <div id="request" className=" mx-4 d-none">
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
                        </div> */}
                      </div>
                    </div>

                    {/* <div
                      className="card bg-light col-11 mx-auto text-dark mt-5"
                      style={{ borderRadius: "1rem" }}
                    >
                      <div className="col-md-4">
                        <h1>{employee.name} Details</h1>
                        <p>Name: {employee.name}</p>
                        <p>Email: {employee.email}</p>
                      </div>
                    </div> */}
                    {/* <div className="container">
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
                    </div> */}
                    {/* ------------------------------------------------------------------------- */}
                  </div>
                </div>
              </div>
            </div>
          </section>
        </div>
      ) : (
        <p>Loading employee details...</p>
      )}
    </div>
  );
}
