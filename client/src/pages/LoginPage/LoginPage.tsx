import {useState} from "react";
import {useAppDispatch, useAppSelector} from "../../app/hooks.ts";
import AuthorizationForm from "../../components/AuthorizationForm/AuthorizationForm.tsx";
import type {UserAuthorization} from "../../components/AuthorizationForm/authorizationModel.ts";
import RegistrationForm from "../../components/RegistrationForm/RegistrationForm.tsx";
import type {UserRegistration} from "../../components/RegistrationForm/registrationModel.ts";
import {loginUser, registerUser} from "../../features/user/userSlice.ts";
import urfuLogo from "../../shared/assets/urfu.png";
import urfuLogoSvg from "../../shared/assets/urfu.svg";
import styles from "./LoginPage.module.scss";

enum states {
	authorization = 0,
	registration = 1,
}

const LoginPage = () => {
	const [state, setState] = useState<states>(states.authorization);
	const [error, setError] = useState("");
	const currentError = useAppSelector((state) => state.user.error);

	const dispatch = useAppDispatch();

	//TODO: fix component reload during auth/registration
	const handleRegistration = async (user: UserRegistration) => {
		try {
			await dispatch(registerUser(user)).unwrap();
		} catch (err) {
			console.error("Failed to register:", err);
			setError(currentError ?? "Ошибка регистрации");
		}
	};

	const handleAuthorization = async (user: UserAuthorization) => {
		try {
			await dispatch(loginUser(user)).unwrap();
		} catch (err) {
			console.error("Failed to login:", err, "current error", currentError);
			setError(currentError ?? "Ошибка авторизации");
		}
	};

	return (
		<div className={styles.loginPageContainer}>
			<picture className={styles.logo}>
				<source srcSet={urfuLogoSvg} type="image/svg+xml" />
				<img src={urfuLogo} alt="logo" />
			</picture>
			{state === states.authorization ? (
				<section className={styles.authorizationForm}>
					<AuthorizationForm
						changeForm={() => {
							setState(states.registration)
						}}
						error={error}
						onSubmit={(user: UserAuthorization) => handleAuthorization(user)}
					/>
				</section>
			) : (
				<section className={styles.registrationForm}>
					<RegistrationForm
						changeForm={() => setState(states.authorization)}
						error={error}
						onSubmit={(user: UserRegistration) => handleRegistration(user)}
					/>
				</section>
			)}
		</div>
	);
};

export default LoginPage;