import { useEffect } from "react";
import { useOktaAuth } from "@okta/okta-react";
import { useNavigate } from "react-router-dom";
import httpClient from "../Api/HttpClient";
import AuthContext from "../../context/AuthProvider";
import useAuth from "../hooks/useAuth";

function AuthCallback() {
  const { oktaAuth, authState } = useOktaAuth();
  const navigate = useNavigate();
  const authContext = useAuth(AuthContext);

  useEffect(() => {
    const handleOktaCallback = async () => {
      try {
        const tokens = await oktaAuth.handleLoginRedirect();
        const { accessToken, idToken } = tokens;
        console.log(tokens);
        localStorage.setItem("accessToken", accessToken.accessToken);
        localStorage.setItem("idToken", idToken.idToken);
        // navigate("/userRequest", { replace: true });
      } catch (err) {
        console.error("Error handling Okta redirect:", err);
        navigate("/login", { replace: true });
      }
    };

    handleOktaCallback();
  }, [oktaAuth, navigate]);

  if (!authState?.isAuthenticated) {
    return <div>Loading...</div>;
  }

  return null;
}

export default AuthCallback;
