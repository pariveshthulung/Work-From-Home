import { useEffect, useState, useLayoutEffect } from "react";
import "bootstrap/dist/css/bootstrap.css";
import httpClient from "../Api/HttpClient";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

export default function SubmitRequest() {
  const [email, setEmail] = useState("");
  const [fromDate, setFromDate] = useState();
  const [toDate, setToDate] = useState();
  const [success, isSuccess] = useState(false);
  const [selectedRequestedType, setSelectedRequestedType] = useState(null);
  const [requestedTypeEnum, setRequestedTypeEnum] = useState([]);
  const navigate = useNavigate();

  useEffect((e) => {
    fetch("https://localhost:7058/api/Enum/getRequestedType")
      .then((response) => response.json())
      .then((data) => {
        setRequestedTypeEnum(data);
      })
      .catch((e) => console.log(e));
  }, []);

  function handleSubmitRequest(e) {
    e.preventDefault();
    console.log(email, fromDate, toDate, selectedRequestedType);
    const postRequest = async () => {
      try {
        await httpClient
          .post("/api/Request/submitrequest", {
            requestedToEmail: email,
            requestedTypeId: selectedRequestedType,
            toDate: toDate,
            fromDate: fromDate,
          })
          .finally(isSuccess(true));
        // toast.success("Requested submitted successfully!!!");
        // navigate("/userRequest", { replace: true });
        navigate("/userRequest", {
          state: { showToast: "Requested submitted successfully!!!" },
        });
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
    var response = postRequest();
  }
  return (
    <div className="container">
      <ToastContainer />

      <div className="row justify-content-center">
        <div className="col-12 text-center mb-4">
          <h1>Submit Requests</h1>
        </div>
        <div className="col-md-4">
          <form>
            <div className="form-row">
              <div className="row">
                <div className="form-group col-md-4">
                  <label htmlFor="inputEmail4">FromDate</label>
                  <input
                    type="date"
                    min={new Date().toISOString().split("T")[0]}
                    value={fromDate}
                    onChange={(e) => setFromDate(e.target.value)}
                  />
                </div>
                <div className="form-group col-md-4">
                  <label htmlFor="inputEmail4">ToDate</label>
                  <input
                    type="date"
                    min={new Date().toISOString().split("T")[0]}
                    value={toDate}
                    onChange={(e) => setToDate(e.target.value)}
                  />
                </div>
              </div>
              <div className="form-group col-md-4">
                <label htmlFor="inputEmail4">Email (To Whome)</label>
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  // className="form-control"
                />
              </div>
            </div>

            <div className="row">
              <div className="form-group col-md-4">
                <label htmlFor="dropdown">Select an option:</label>
                <select
                  id="dropdown"
                  value={selectedRequestedType}
                  onChange={(e) => setSelectedRequestedType(e.target.value)}
                >
                  <option value="">Select an option</option>
                  {requestedTypeEnum.map((item) => (
                    <option key={item.id} value={item.id}>
                      {item.type}
                    </option> // Assuming each item has id, value, and name
                  ))}
                </select>
              </div>
            </div>
            <button
              type="submit"
              onClick={handleSubmitRequest}
              className="btn btn-primary my-3"
            >
              Submit Request
            </button>
            {/* <Link to="/listRequest"> */}
            <button
              // type="submit"
              onClick={() => navigate("/listRequest")}
              className="btn btn-secondary my-3 mx-4"
            >
              Back
            </button>
            {/* </Link> */}
          </form>
        </div>
      </div>
    </div>
  );
}
