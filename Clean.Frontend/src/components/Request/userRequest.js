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
      const response = await httpClient.get(
        `/api/Request/getAllUserRequestQuery?email=${auth.email}`
      );
      setRequests(response.data); // Assuming response.data contains the list of requests
      console.log("Requests: ", response.data);
    } catch (err) {
      console.error(err);
    }
  }, []);

  useEffect(() => {
    fetchUserData();
  }, [fetchUserData]);

  return (
    <div>
      {/* <ToastContainer /> */}
      <Request
        heading={"My Request"}
        requests={requests}
        button={<Button name="Submit Request" goto="/submitRequest" />}
      />
    </div>
  );
}
