import { useNavigate } from "react-router-dom";

const RequestDropdown = ({ active, setActive }) => {
  const navigate = useNavigate();

  const closeDropdown = () => {
    const dropdownToggle = document.getElementById("dropdownMenuLink");
    const dropdown = new window.bootstrap.Dropdown(dropdownToggle);
    dropdown.hide();
  };

  return (
    <div className="dropdown my-auto">
      <a
        href="#"
        role="button"
        id="dropdownMenuLink"
        data-bs-toggle="dropdown"
        aria-expanded="false"
        // className="nav-link dropdown-toggle"
        className={
          active === "Request"
            ? "nav-link dropdown-toggle active"
            : "nav-link dropdown-toggle"
        }
      >
        Request
      </a>
      <ul className="dropdown-menu" aria-labelledby="dropdownMenuLink">
        <li>
          <a
            style={{ cursor: "pointer" }}
            className="dropdown-item"
            onClick={() => {
              setActive("Request");
              navigate("/approveRequest");
              closeDropdown();
            }}
          >
            Approve Request
          </a>
        </li>
        <li>
          <a
            style={{ cursor: "pointer" }}
            className="dropdown-item"
            onClick={() => {
              setActive("Request");
              navigate("/userRequest");
              closeDropdown();
            }}
          >
            My Request
          </a>
        </li>
        <li>
          <a
            style={{ cursor: "pointer" }}
            className="dropdown-item"
            onClick={() => {
              setActive("Request");
              navigate("/listRequest");
              closeDropdown();
            }}
          >
            All Request
          </a>
        </li>
      </ul>
    </div>
  );
};

export default RequestDropdown;
