import { useEffect, useState } from "react";
import "jquery/dist/jquery.min.js";
import "bootstrap/dist/js/bootstrap.min.js";
import "bootstrap/dist/js/bootstrap.bundle";
import "bootstrap/dist/js/bootstrap.bundle.min";
import { useNavigate, useLocation } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import AuthContext from "../../context/AuthProvider";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import httpClient from "../Api/HttpClient";
import { data } from "jquery";
import { delay } from "@okta/okta-auth-js";

function EmployeeIndex({ button }) {
  const navigate = useNavigate();
  const [employee, setEmployee] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [orderColumn, setOrderColumn] = useState("");
  const [orderBy, setOrderBy] = useState("");
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(5);
  const [totalCount, setTotalCount] = useState(0);
  const [nextPageExist, setNextPageExist] = useState(false);
  const [previousPageExist, setPreviousPageExist] = useState(false);

  const [loading, setLoading] = useState(true);
  const [selectedEmployeeId, SetSelectedEmployeeId] = useState(null);
  const location = useLocation();

  const { auth } = useAuth(AuthContext);

  useEffect(() => {
    if (location.state?.showToast) {
      toast.success(location.state?.showToast);
    }
    navigate(location.pathname, { replace: true });
  }, []);
  const fetchEmployees = async () => {
    try {
      var response = await httpClient.get(
        `/api/Employee/getAllEmployee?searchTerm=${searchTerm}&sortOrder=${orderBy}&sortColumn=${orderColumn}&page=${page}&pageSize=${pageSize}`
      );
      console.log(response);
      setTotalCount(response.data.totalCount);
      setEmployee(response.data.items);
      setNextPageExist(response.data.nextPageExist);
      setPreviousPageExist(response.data.previousPageExist);
      setLoading(false);
      console.log(previousPageExist);
      console.log(nextPageExist);
    } catch (error) {
      console.log(error);
    }
  };
  useEffect(() => {
    fetchEmployees();
  }, [page, pageSize]);

  function handleSearch(e) {
    e.preventDefault();
    fetchEmployees();
  }
  const handleNext = () => {
    if (nextPageExist) {
      setPage(page + 1);
    }
  };

  const handlePrevious = () => {
    if (previousPageExist) {
      setPage(page - 1);
    }
  };

  const startEntry = (page - 1) * pageSize + 1;
  const endEntry = Math.min(page * pageSize, totalCount);

  const pageOptions = [5, 10, 15, 20];

  useEffect(() => {
    fetchEmployees();
  }, []);

  const handleRowClick = (id) => {
    navigate(`/employeeDetails/${id}`);
    console.log("clicked");
  };
  const handleEditClick = (id) => {
    navigate(`/editEmployee/${id}`);
  };

  const handleDeleteConfimation = async (id) => {
    console.log("deleyed clicked");
    console.log(id);
    try {
      var response = await httpClient.delete(
        `/api/Employee/deleteEmployee?employeeId=${id}`
      );
      console.log(response);

      if (response.status === 204) {
        setEmployee((employee) => employee.filter((x) => x.guidId !== id));
        toast.success("Employee deleted successfully.");
      }
      // setDeleteId(null);
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
    !loading && (
      <div className="container">
        <ToastContainer />
        <div className="row justify-content-center">
          <div className="col-12 text-center">
            <h1>Employees</h1>
          </div>
          <div className="col-12">
            <div className="mx-5 mt-3">
              {/* <div className="d-flex justify-content-end ">
              <Link to="/addEmployee">
                <button type="button" className="btn btn-primary my-2">
                  Add
                </button>
              </Link>
            </div> */}

              {/* <div className="col-12">
                <div>
                  <form class="d-flex col-6">
                    <input
                      class="form-control me-2"
                      type="search"
                      placeholder="Search"
                      aria-label="Search"
                    />
                    <button class="btn btn-outline-success" type="submit">
                      Search
                    </button>
                  </form>
                </div>

                <div>{button}</div>
              </div> */}
              <div className="row">
                <div className="col-md-2 my-auto">
                  show
                  <select
                    id="dropdown"
                    value={pageSize}
                    onChange={(e) => setPageSize(e.target.value)}
                  >
                    {/* <option value={pageSize}>Select an option</option> */}
                    {pageOptions.map((option) => (
                      <option key={option} value={option}>
                        {option}
                      </option>
                    ))}
                  </select>
                  entries
                </div>
                <div className="col-md-6 my-auto">
                  <form className="d-flex col-6">
                    <input
                      className="form-control me-2"
                      type="search"
                      placeholder="Search"
                      aria-label="Search"
                      onChange={(e) => setSearchTerm(e.target.value)}
                    />
                    <button
                      className="btn btn-outline-success"
                      onClick={(e) => handleSearch(e)}
                    >
                      Search
                    </button>
                  </form>
                </div>
                <div className="col-md-4 text-end">{button}</div>
              </div>

              <table className="table table-striped">
                <thead>
                  <tr>
                    <th scope="col">S.N.</th>
                    <th scope="col">Email</th>
                    <th scope="col">Name</th>
                    <th scope="col">Role</th>
                  </tr>
                </thead>
                <tbody>
                  {employee.map((employee, i) => (
                    <tr style={{ cursor: "pointer" }} key={employee.id}>
                      <td onClick={() => handleRowClick(employee.guidId)}>
                        {i + 1}
                      </td>
                      <td onClick={() => handleRowClick(employee.guidId)}>
                        {employee.email}
                      </td>
                      <td onClick={() => handleRowClick(employee.guidId)}>
                        {employee.name}
                      </td>
                      <td onClick={() => handleRowClick(employee.guidId)}>
                        {employee.userRole}
                      </td>
                      {((!loading && auth.userRole === "Admin") ||
                        auth.userRole === "SuperAdmin" ||
                        auth.userRole === "Manager" ||
                        auth.userRole === "Ceo") && (
                        <>
                          <td>
                            <svg
                              xmlns="http://www.w3.org/2000/svg"
                              width="16"
                              height="16"
                              fill="currentColor"
                              className="bi bi-pencil-square"
                              viewBox="0 0 16 16"
                              onClick={() => {
                                handleEditClick(employee.guidId);
                              }}
                            >
                              <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z" />
                              <path
                                fillRule="evenodd"
                                d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"
                              />
                            </svg>
                          </td>
                          <td>
                            <svg
                              onClick={() =>
                                SetSelectedEmployeeId(employee.guidId)
                              }
                              data-bs-toggle="modal"
                              data-bs-target="#exampleModal"
                              xmlns="http://www.w3.org/2000/svg"
                              width="16"
                              height="16"
                              fill="currentColor"
                              className="bi bi-trash"
                              viewBox="0 0 16 16"
                            >
                              <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z" />
                              <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z" />
                            </svg>
                          </td>
                        </>
                      )}
                    </tr>
                  ))}
                </tbody>
              </table>
              <div className="d-flex justify-content-between align-items-center mt-3">
                <div>
                  Showing {startEntry} to {endEntry} of {totalCount} entries
                </div>
                <nav>
                  <ul className="pagination mb-0">
                    <li className="page-item">
                      <button
                        className="page-link"
                        onClick={handlePrevious}
                        // disabled={!previousPageExist}
                      >
                        Previous
                      </button>
                    </li>
                    <li className="page-item active">
                      <button className="page-link">{page}</button>
                    </li>
                    <li className="page-item">
                      <button
                        className="page-link"
                        onClick={handleNext}
                        // disabled={!nextPageExist}
                      >
                        Next
                      </button>
                    </li>
                  </ul>
                </nav>
              </div>

              {/* lunch model */}
              <div
                className="modal fade"
                id="exampleModal"
                aria-labelledby="exampleModalLabel"
                aria-hidden="true"
              >
                <div className="modal-dialog">
                  <div className="modal-content">
                    <div className="modal-header">
                      <h5 className="modal-title" id="exampleModalLabel">
                        Warning!!!
                      </h5>
                      <button
                        type="button"
                        className="btn-close"
                        data-bs-dismiss="modal"
                        aria-label="Close"
                      ></button>
                    </div>
                    <div className="modal-body">Do you want to delete?</div>
                    <div className="modal-footer">
                      <button
                        type="button"
                        className="btn btn-secondary"
                        data-bs-dismiss="modal"
                      >
                        Close
                      </button>
                      <button
                        onClick={() =>
                          handleDeleteConfimation(selectedEmployeeId)
                        }
                        type="button"
                        className="btn btn-primary"
                        data-bs-dismiss="modal"
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                </div>
              </div>
              {/*  */}
            </div>
          </div>
        </div>
      </div>
    )
  );
}
export default EmployeeIndex;
