import { useNavigate } from "react-router-dom";
import { useContext } from "react";
import AuthContext from "../../context/AuthProvider";

const UserProfileComponent = () => {
  const authContext = useContext(AuthContext);
  const navigate = useNavigate();

  return (
    <div className="mx-3">
      <div className="dropdown">
        <span className="mx-2">{authContext.auth.name}</span>
        <svg
          href="#"
          role="button"
          id="dropdownMenuLink"
          data-bs-toggle="dropdown"
          aria-expanded="false"
          xmlns="http://www.w3.org/2000/svg"
          width="20"
          height="20"
          fill="currentColor"
          className="bi bi-person-lines-fill"
          viewBox="0 0 16 16"
        >
          <path d="M6 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6m-5 6s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1zM11 3.5a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5m.5 2.5a.5.5 0 0 0 0 1h4a.5.5 0 0 0 0-1zm2 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1zm0 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1z" />
        </svg>
        <ul style={{ cursor: "pointer" }} className="dropdown-menu">
          <li>
            <a
              className="dropdown-item"
              onClick={() => {
                navigate("/userProfile");
              }}
            >
              Profile
            </a>
          </li>
          <li>
            <a
              className="dropdown-item"
              onClick={() => {
                window.localStorage.clear();
                // authContext.setAuth(null);
                navigate("/login");
              }}
            >
              Logout
            </a>
          </li>
        </ul>
      </div>
    </div>
  );
};

export default UserProfileComponent;
