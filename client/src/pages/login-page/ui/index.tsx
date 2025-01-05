import {useState} from "react";
import {useNavigate} from "react-router-dom";
import urfuLogo from "shared/assets/urfu.png";
import {useAppDispatch, useAppSelector} from "../../../app/hooks.ts";
import AuthorizationForm from "../../../components/AuthorizationForm/AuthorizationForm.tsx";
import type {UserAuthorization} from "../../../components/AuthorizationForm/authorizationModel.ts";
import RegistrationForm from "../../../components/RegistrationForm/RegistrationForm.tsx";
import type {UserRegistration} from "../../../components/RegistrationForm/registrationModel.ts";
import {loginUser} from "../../../features/user/userSlice.ts";
import styles from "./LoginPage.module.scss";

enum states {
	authorization = 0,
	registration = 1,
}

const LoginPage = () => {
	const [state, setState] = useState<states>(states.authorization);
	const dispatch = useAppDispatch();
	const error = useAppSelector((state) => state.user.error);

	const handleRegistration = async (user: UserRegistration) => {
		try {
			dispatch(loginUser(user)).unwrap();
		} catch (err) {
			console.error("Failed to register:", err);
		}
	};

	const handleAuthorization = async (user: UserAuthorization) => {
		try {
			dispatch(loginUser(user)).unwrap();
		} catch (err) {
			console.error("Failed to login:", err);
		}
	};

	return (
		<div className={styles.loginPageContainer}>
			<img className={styles.logo} src={urfuLogo} alt="logo" />
			{state === states.authorization ? (
				<section className={styles.authorizationForm}>
					<AuthorizationForm
						changeForm={() => setState(states.registration)}
						onSubmit={(user: UserAuthorization) => handleAuthorization(user)}
					/>
				</section>
			) : (
				<section className={styles.registrationForm}>
					<RegistrationForm
						changeForm={() => setState(states.authorization)}
						onSubmit={(user: UserRegistration) => handleRegistration(user)}
					/>
				</section>
			)}
			{error && <div className={styles.error}>{error}</div>}
		</div>
	);
};

export default LoginPage;