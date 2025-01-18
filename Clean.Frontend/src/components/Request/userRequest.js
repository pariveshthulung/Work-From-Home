import { useEffect, useState, useCallback } from "react";
import Request from "./Request";
import httpClient from "../Api/HttpClient";
import AuthContext from "../../context/AuthProvider";
import useAuth from "../hooks/useAuth";
import Button from "../common/Button";
import { useLocation, useNavigate } from "react-router-dom";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "bootstrap/dist/js/bootstrap.bundle.min";

export default function UserRequest() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(true);
  const location = useLocation();
  const navigate = useNavigate();
  const { auth } = useAuth(AuthContext);

  useEffect(() => {
    if (location.state?.showToast) {
      toast.success(location.state?.showToast);
    }
    navigate(location.pathname, { replace: true, state: null });
  }, []);

  const fetchUserData = useCallback(async () => {
    console.log(auth);
    try {
      const response = await httpClient
        .get(`/api/Request/getAllUserRequestQuery?email=${auth.email}`)
        .finally(setLoading(false));
      setRequests(response.data); // Assuming response.data contains the list of requests
      console.log("Requests: ", response.data);
    } catch (err) {
      console.error(err);
    }
  }, []);

  useEffect(() => {
    fetchUserData();
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
                  <h1>My Requests</h1>
                </div>
                <div>
                  <div className="mb-3">
                    <Request
                      requests={requests}
                      button={
                        <Button name="Submit Request" goto="/submitRequest" />
                      }
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>
    )
  );
}
