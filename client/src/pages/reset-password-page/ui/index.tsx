import { useNavigate } from "react-router-dom";
import {useAppSelector} from "../../../app/hooks.ts";
import ResetPasswordForm from "../../../components/ResetPasswordForm/ResetPasswordForm.tsx";
import { axiosInstance } from "../../../shared/configs/apiConfig.ts";
import styles from "./ResetPasswordPage.module.scss";

const ResetPasswordPage = () => {
	const navigate = useNavigate();
	const user = useAppSelector(s => s.user.user)
	const handleSubmit = (oldPassword: string, newPassword: string) => {
		axiosInstance
			.post("/accounts/change-password", {
				login: user?.login,
				new_password: newPassword,
				old_password: oldPassword,
			})
			.then(() => navigate("/"));
	};

	return (
		<div>
			<section className={styles.gostSection}>
				<ResetPasswordForm handleSubmit={handleSubmit} />
			</section>
		</div>
	);
};

export default ResetPasswordPage;