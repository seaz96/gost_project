import { useContext, useState } from "react";

import { UserContext, type userModel } from "entities/user";
import urfuLogo from "shared/assets/urfu.png";
import { axiosInstance } from "shared/configs/axiosConfig";
import { AuthorizationForm, type authorizationModel } from "widgets/authorization-form";
import { RegistrationForm, type registrationModel } from "widgets/registration-form";
import styles from "./LoginPage.module.scss";

enum states {
	authorization = 0,
	registration = 1,
}

const LoginPage = () => {
	const [state, setState] = useState<states>(states.registration);
	const { setUser } = useContext(UserContext);

	const handleRegistration = (user: registrationModel.UserRegistration) => {
		axiosInstance
			.post<userModel.User>("/accounts/register", user)
			.then((response) => {
				setUser(response.data);
				localStorage.setItem("jwt_token", response.data.token);
				window.location.href = "/";
			})
			.catch((error) => console.log(error));
	};

	const handleAuthorization = (user: authorizationModel.UserAuthorization) => {
		return axiosInstance.post<userModel.User>("/accounts/login", user).then((response) => {
			setUser(response.data);
			localStorage.setItem("jwt_token", response.data.token);
			window.location.href = "/";
			return null;
		});
	};

	return (
		<div className={styles.loginPageContainer}>
			<img className={styles.logo} src={urfuLogo} alt="logo" />
			{state === states.authorization ? (
				<section className={styles.authorizationForm}>
					<AuthorizationForm
						changeForm={() => setState(states.registration)}
						onSubmit={(user: authorizationModel.UserAuthorization) => handleAuthorization(user)}
					/>
				</section>
			) : (
				<section className={styles.registrationForm}>
					<RegistrationForm
						changeForm={() => setState(states.authorization)}
						onSubmit={(user: registrationModel.UserRegistration) => handleRegistration(user)}
					/>
				</section>
			)}
		</div>
	);
};

export default LoginPage;
