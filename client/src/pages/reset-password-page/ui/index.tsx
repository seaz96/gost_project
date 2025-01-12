import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../../app/hooks.ts";
import ResetPasswordForm from "../../../components/ResetPasswordForm/ResetPasswordForm.tsx";
import { useResetPasswordMutation } from "../../../features/api/apiSlice";
import styles from "./ResetPasswordPage.module.scss";

const ResetPasswordPage = () => {
	const navigate = useNavigate();
	const user = useAppSelector((s) => s.user.user);
	const [resetPassword] = useResetPasswordMutation();

	const handleSubmit = async (oldPassword: string, newPassword: string) => {
		//TODO: stay on page if error
		if (!user) {
			return;
		}
		await resetPassword({
			login: user.login,
			old_password: oldPassword,
			new_password: newPassword,
		});
		navigate("/");
	};

	return (
		<div className="container">
			<h1 className="verticalPadding">Сброс пароля</h1>
			<section className={styles.gostSection}>
				<ResetPasswordForm handleSubmit={handleSubmit} />
			</section>
		</div>
	);
};

export default ResetPasswordPage;
