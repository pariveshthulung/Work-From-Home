import { useNavigate } from "react-router-dom";
export default function Button({ name, goto }) {
  const navigate = useNavigate();
  return (
    <div className="d-flex justify-content-end ">
      <button
        type="button"
        onClick={() => navigate(goto)}
        className="btn btn-primary my-2"
      >
        {name}
      </button>
    </div>
  );
}
