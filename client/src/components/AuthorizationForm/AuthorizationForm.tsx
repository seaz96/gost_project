import { useFormik } from "formik";
import { Button, Input } from "../../shared/components";
import styles from "./AuthorizationForm.module.scss";
import type { UserAuthorization } from "./authorizationModel.ts";

interface AuthorizationFormProps {
	changeForm: () => void;
	onSubmit: (user: UserAuthorization) => Promise<void>;
	error: string | null;
}

const AuthorizationForm: React.FC<AuthorizationFormProps> = (props) => {
	const { changeForm, onSubmit, error } = props;

	const validate = (values: { login: string; password: string }) => {
		const errors: { login?: string; password?: string } = {};

		if (!values.login) {
			errors.login = "Заполните поле";
		} /* else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.login)) {
      errors.login = 'Некорректный логин';
    } */

		if (values.password.length < 7) {
			errors.password = "Пароль должен быть не меньше 7 символов";
		}

		return errors;
	};

	const formik = useFormik({
		initialValues: {
			login: "",
			password: "",
		},
		validate,
		onSubmit: (values) => {
			onSubmit(values);
		},
	});

	return (
		<form
			className={styles.form}
			onSubmit={(event) => {
				event.preventDefault();
				formik.handleSubmit();
			}}
		>
			<Input
				type="text"
				id="login"
				name="login"
				label="Логин"
				placeholder="user@example.com"
				onChange={formik.handleChange("login")}
				value={formik.values.login}
				error={formik.errors.login}
			/>
			<Input
				type="password"
				label="Пароль"
				id="password"
				name="password"
				placeholder=""
				value={formik.values.password}
				onChange={formik.handleChange("password")}
				error={formik.errors.password}
			/>
			{error && <p className={styles.error}>{error}</p>}
			<div className={styles.buttonsContainer}>
				<Button isFilled className={styles.formButton} type="submit">
					Войти
				</Button>
				<Button className={styles.formButton} onClick={() => changeForm()} isColoredText>
					Зарегистрироваться
				</Button>
			</div>
		</form>
	);
};

export default AuthorizationForm;