import { useForm } from "react-hook-form";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import UrfuTextInput from "../../shared/components/Input/UrfuTextInput.tsx";
import styles from "./AuthorizationForm.module.scss";
import type { UserAuthorization } from "./authorizationModel.ts";

interface AuthorizationFormProps {
	changeForm: () => void;
	onSubmit: (user: UserAuthorization) => Promise<void>;
	error: string | null;
}

const AuthorizationForm: React.FC<AuthorizationFormProps> = (props) => {
	const { changeForm, onSubmit, error } = props;

	const {
		register,
		handleSubmit,
		formState: { errors },
	} = useForm<UserAuthorization>({
		defaultValues: {
			login: "",
			password: "",
		},
	});

	return (
		<form className={styles.form} onSubmit={handleSubmit(onSubmit)}>
			<UrfuTextInput
				type="text"
				id="login"
				label="Логин"
				placeholder="user@example.com"
				{...register("login", {
					required: "Заполните поле",
				})}
				error={errors.login?.message}
			/>
			<UrfuTextInput
				type="password"
				label="Пароль"
				id="password"
				placeholder=""
				{...register("password", {
					required: "Заполните поле",
					minLength: {
						value: 7,
						message: "Пароль должен быть не меньше 7 символов",
					},
				})}
				error={errors.password?.message}
			/>
			{error && <p className={styles.error}>{error}</p>}
			<div className={styles.buttonsContainer}>
				<UrfuButton type="submit">Войти</UrfuButton>
				<UrfuButton onClick={() => changeForm()} outline={true}>
					Зарегистрироваться
				</UrfuButton>
			</div>
		</form>
	);
};

export default AuthorizationForm;
