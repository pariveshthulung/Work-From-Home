import { useEffect, useState } from "react";
import Request from "./Request";
import httpClient from "../Api/HttpClient";
import Button from "../common/Button";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import DropdownComponent from "../common/DropdownComponent";
import DeleteIcon from "../common/DeleteIcon";

export default function ListRequest() {
  const [requests, setRequests] = useState([]);

  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    if (location.state?.showToast) {
      toast.success(location.state?.showToast);
    }
  }, []);

  async function fetchRequest() {
    try {
      var requests = await httpClient.get("/api/Request/getAllRequest");
      setRequests(requests.data);
      console.log(requests);
    } catch (err) {
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
    <section className="vh-100 gradient-custom">
      <div className="container py-5">
        <div className="row  d-flex justify-content-center h-100">
          <div className="col-12">
            <div
              className="card bg-light text-dark"
              style={{ borderRadius: "1rem" }}
            >
              <div className="mt-4 mx-5 text-center">
                <h1>Requests</h1>
              </div>
              <div className="mb-3">
                <Request
                  requests={requests}
                  setRequests={setRequests}
                  fetchRequest={fetchRequest}
                  DropdownComponent={DropdownComponent}
                  DeleteIcon={DeleteIcon}
                  button={
                    <Button name="Submit Request" goto="/submitRequest" />
                  }
                />
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
