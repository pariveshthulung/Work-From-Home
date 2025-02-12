import { useEffect, useState } from "react";
import httpClient from "../Api/HttpClient";
import Request from "./Request";
import useAuth from "../hooks/useAuth";
import AuthContext from "../../context/AuthProvider";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import DropdownComponent from "../common/DropdownComponent";
import DeleteIcon from "../common/DeleteIcon";

export default function PendingRequest() {
  const [loading, setLoading] = useState(false);
  const [requests, setRequests] = useState(false);
  const { auth } = useAuth(AuthContext);

  async function fetchRequest() {
    try {
      var response = await httpClient
        .get(`/api/Request/getAllRequestSubmitToUser?email=${auth.email}`)
        .finally(setLoading(false));
      console.log(response);
      setRequests(response.data);
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
  }
  useEffect(() => {
    fetchRequest();
  }, []);

  return (
    !loading && (
      <section className="vh-100 gradient-custom">
        <div className="container py-5">
          <div className="row  d-flex justify-content-center h-100">
            <div className="col-12">
              <div
                className="card bg-light text-dark"
                style={{ borderRadius: "1rem" }}
              >
                <div className="mt-4 mx-5 text-center">
                  <h1>Approve Requests</h1>
                </div>
                <div className="mb-3">
                  <Request
                    requests={requests}
                    fetchRequest={fetchRequest}
                    setRequests={setRequests}
                    DropdownComponent={DropdownComponent}
                    DeleteIcon={DeleteIcon}
                  />
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    )
  );
}
