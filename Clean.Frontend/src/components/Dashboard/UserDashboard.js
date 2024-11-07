import { useEffect, useState } from "react";
import "bootstrap/dist/css/bootstrap.css";
import { useNavigate } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import AuthContext from "../../context/AuthProvider";
import Request from "../Request/Request";
import httpClient from "../Api/HttpClient";
import UserRequest from "../Request/userRequest";
import Button from "../common/Button";

export default function UserDashboard({ button }) {
  const navigate = useNavigate();
  const [requests, setRequests] = useState([]);
  const authContext = useAuth(AuthContext);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchRequest() {
      console.log(authContext.auth);
      var response = await httpClient
        .get(
          `/api/Request/getAllUserRequestQuery?email=${authContext.auth.email}`
        )
        .finally(setLoading(false));
      setRequests(response.data);
      console.log(response);
    }
    fetchRequest();
  }, []);
  if (loading)
    return (
      <div>
        <p>loading...343</p>
      </div>
    );
  return (
    !loading && (
      <Request
        requests={requests}
        button={<Button name="Submit Request" goto="/submitRequest" />}
      />
    )
  );
  // <UserRequest />;
}
