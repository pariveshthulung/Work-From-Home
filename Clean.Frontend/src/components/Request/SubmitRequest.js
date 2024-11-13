import { useEffect, useState, useLayoutEffect } from "react";
import "bootstrap/dist/css/bootstrap.css";
import httpClient from "../Api/HttpClient";
import { useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export default function SubmitRequest() {
  const [email, setEmail] = useState("");
  const [description, setDescription] = useState("");
  const [managersEmail, setManagersEmail] = useState([]);
  const [fromDate, setFromDate] = useState();
  const [toDate, setToDate] = useState();

  const navigate = useNavigate();

  useEffect((e) => {
    fetch("https://localhost:7058/api/Employee/getManagersEmail")
      .then((response) => response.json())
      .then((data) => {
        setManagersEmail(data);
      })
      .catch((e) => console.log(e));
  }, []);

  function handleSubmitRequest(e) {
    e.preventDefault();
    console.log(email, fromDate, toDate);
    const postRequest = async () => {
      try {
        await httpClient.post("/api/Request/submitrequest", {
          requestedToEmail: email,
          toDate: toDate,
          fromDate: fromDate,
          description: description,
        });
        navigate("/userRequest", {
          state: { showToast: "Requested submitted successfully!!!" },
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
    postRequest();
  }
  return (
    <section className="vh-100 gradient-custom">
      <div className="container py-5 h-100">
        <ToastContainer />
        <div className="row d-flex justify-content-center align-items-center">
          <div className="col-12 col-md-8 col-lg-6 col-xl-5">
            <div
              className="card bg-light text-dark"
              style={{ borderRadius: "1rem" }}
            >
              <div className="card-body p-5 text-center">
                <div className="container">
                  <ToastContainer />

                  <div className="row justify-content-center">
                    <div className="col-12 text-center mb-4">
                      <h1>Submit Requests</h1>
                    </div>
                    <div className="">
                      <form>
                        <div className="form-group mb-4">
                          <label
                            htmlFor="inputEmail4"
                            className="mb-2 form-label"
                          >
                            FromDate
                          </label>
                          <input
                            type="date"
                            min={new Date().toISOString().split("T")[0]}
                            value={fromDate}
                            onChange={(e) => setFromDate(e.target.value)}
                            className="form-control"
                          />
                        </div>
                        <div className="form-group mb-4">
                          <label htmlFor="inputEmail4" className="mb-2">
                            ToDate
                          </label>
                          <input
                            type="date"
                            min={new Date().toISOString().split("T")[0]}
                            value={toDate}
                            onChange={(e) => setToDate(e.target.value)}
                            className="form-control"
                          />
                        </div>

                        <div className="row">
                          <div className="form-group mb-4">
                            <label htmlFor="dropdown" className="mb-2">
                              Request to (email):
                            </label>
                            <select
                              id="dropdown"
                              value={email}
                              className="form-control"
                              onChange={(e) => setEmail(e.target.value)}
                              required
                            >
                              <option value="">Select an Email</option>
                              {managersEmail.map((item, i) => (
                                <option key={i} value={item}>
                                  {item}
                                </option> // Assuming each item has id, value, and name
                              ))}
                            </select>
                          </div>
                        </div>
                        <div className="form-group mb-4">
                          <label htmlFor="inputEmail4" className="mb-2">
                            Description
                          </label>
                          <textarea
                            type="email"
                            value={email}
                            onChange={(e) => setDescription(e.target.value)}
                            className="form-control"
                          />
                        </div>
                        <hr className="my-4" />

                        <button
                          type="submit"
                          onClick={handleSubmitRequest}
                          className="btn btn-primary my-3"
                        >
                          Submit Request
                        </button>
                        <button
                          onClick={() => navigate("/listRequest")}
                          className="btn btn-secondary my-3 mx-4"
                        >
                          Back
                        </button>
                      </form>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
