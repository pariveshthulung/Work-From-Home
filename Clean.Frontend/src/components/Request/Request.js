import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.css";
import httpClient from "../Api/HttpClient";
import "jquery/dist/jquery.min.js";
import "bootstrap/dist/js/bootstrap.min.js";
import "bootstrap/dist/js/bootstrap.bundle";
import "bootstrap/dist/js/bootstrap.bundle.min";
import useAuth from "../hooks/useAuth";
import AuthContext from "../../context/AuthProvider";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
export default function Request({
  heading,
  requests,
  setRequests,
  fetchRequest,
  button,
  DropdownComponent,
  DeleteIcon,
}) {
  const [deleteId, setDeleteId] = useState(null);
  const [requestId, setRequestId] = useState(null);
  const [employeeId, setEmployeeId] = useState(null);
  const [status, setStatus] = useState([]);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  const { auth } = useAuth(AuthContext);
  // ------------------------------------------------------------------------------------------------

  // filter status based on status on request

  // ------------------------------------------------------------------------------------------------
  useEffect(() => {
    async function fetchStatus() {
      try {
        var response = await httpClient.get(
          "https://localhost:7058/api/Enum/getApprovalStatus"
        );
        setStatus(response.data);
      } catch (err) {
        setError(err.response.data);
      }
    }
    fetchStatus();
  }, []);

  const handleOnDeleteClick = (employeeId, requestId) => {
    setRequestId(requestId);
    setEmployeeId(employeeId);
    // console.log(id);
  };

  const handleStatusClick = async (employeeId, requestId, statusId) => {
    // console.log(status);
    // console.log(id);
    try {
      var response = await httpClient.put(`/api/Request/approverequest`, {
        employeeId: employeeId,
        approvalStatusId: statusId,
        requestId: requestId,
      });
      // .finally(() => setLoading(true));
      console.log(response);
      toast.success("Request status changed sucessfully!!");
      await fetchRequest();
      // console.log(newRequests);
    } catch (err) {
      console.log(err);
      if (err.response && Array.isArray(err.response.data.errors)) {
        err.response.data.errors.forEach((errorObj) => {
          toast.error(`${errorObj.field}: ${errorObj.message}`);
        });
      } else {
        toast.error("something went wrong!!!");
      }
    }
  };

  const handleDeleteConfimation = async () => {
    console.log("deleyed clicked");
    try {
      var response = await httpClient.delete(
        `/api/Request/deleteRequest?employeeId=${employeeId}&requestId=${requestId}`
      );

      if (response.status === 204) {
        setRequests((requests) =>
          requests.filter((x) => x.guidId !== requestId)
        );
        toast.success("Request deleted successfully.");
      }
      setDeleteId(null);
    } catch (err) {
      console.log(err);
      if (err.response && Array.isArray(err.response.data.errors)) {
        err.response.data.errors.forEach((errorObj) => {
          toast.error(`${errorObj.field}: ${errorObj.message}`);
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
            <h1>{heading}</h1>
          </div>
          <div className="col-12">
            <div className="mx-5 mt-3">
              {button}

              <table className="table table-striped">
                <thead>
                  <tr>
                    <th scope="col">S.N.</th>
                    <th scope="col">Request By</th>
                    <th scope="col">Request To</th>
                    <th scope="col">Type</th>
                    <th scope="col">FromDate</th>
                    <th scope="col">ToDate</th>
                    <th scope="col">Status</th>
                  </tr>
                </thead>
                <tbody>
                  {requests.length > 0 ? (
                    requests.map((request, i) => (
                      <tr key={request.id}>
                        <td>{i + 1}</td>
                        <td>{request.requestedByEmail}</td>
                        <td>{request.requestedToEmail}</td>
                        <td>{request.requestedType}</td>
                        <td>
                          {
                            new Date(request.fromDate + "z")
                              .toISOString()
                              .split("T")[0]
                          }
                        </td>
                        <td>
                          {
                            new Date(request.toDate + "z")
                              .toISOString()
                              .split("T")[0]
                          }
                        </td>
                        <td>{request.approval.approvalStatus}</td>
                        {/* put conditions */}
                        {!loading &&
                          (auth.userRole === "Admin" ||
                            auth.userRole === "SuperAdmin" ||
                            auth.userRole === "Manager" ||
                            auth.userRole === "Ceo") && (
                            <>
                              {/* Pass DropdownComponent */}
                              {DropdownComponent && (
                                <DropdownComponent
                                  request={request}
                                  status={status}
                                  handleStatusClick={handleStatusClick}
                                />
                              )}

                              {/* Pass DeleteIcon */}
                              {DeleteIcon && (
                                <DeleteIcon
                                  employeeId={request.employeeGuidId}
                                  requestId={request.guidId}
                                  handleOnDeleteClick={handleOnDeleteClick}
                                />
                              )}
                            </>
                          )}
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="7">No data available</td>
                    </tr>
                  )}
                </tbody>
              </table>
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
                        onClick={() => setDeleteId(null)}
                        type="button"
                        className="btn btn-secondary"
                        data-bs-dismiss="modal"
                      >
                        Close
                      </button>
                      <button
                        onClick={() => handleDeleteConfimation()}
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
