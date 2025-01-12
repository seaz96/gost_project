import type { FetchBaseQueryError } from "@reduxjs/toolkit/query";
import { useState } from "react";
import AuthorizationForm from "../../components/AuthorizationForm/AuthorizationForm.tsx";
import type { UserAuthorization } from "../../components/AuthorizationForm/authorizationModel.ts";
import RegistrationForm from "../../components/RegistrationForm/RegistrationForm.tsx";
import type { UserRegistration } from "../../components/RegistrationForm/registrationModel.ts";
import { useLoginUserMutation, useRegisterUserMutation } from "../../features/api/apiSlice.ts";
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

	const [loginUser, { error: loginError }] = useLoginUserMutation();
	const [registerUser] = useRegisterUserMutation();

	//TODO: fix component reload during auth/registration
	const handleRegistration = async (user: UserRegistration) => {
		try {
			await registerUser(user).unwrap();
		} catch (err) {
			console.error("Failed to register:", err);
		}
	};

	const handleAuthorization = async (user: UserAuthorization) => {
		try {
			await loginUser(user).unwrap();
		} catch (err) {
			console.error("Failed to login:", err, loginError);
			if ((err as FetchBaseQueryError).status === 400) {
				setError("Неверный логин или пароль");
			}
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
							setState(states.registration);
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