import { useForm } from "react-hook-form";
import UrfuButton from "../../shared/components/Button/UrfuButton.tsx";
import UrfuTextInput from "../../shared/components/Input/UrfuTextInput.tsx";
import styles from "./ResetPasswordForm.module.scss";

interface ResetPasswordFormProps {
	handleSubmit: (oldPassword: string, newPassword: string) => void;
}

interface FormData {
	oldPassword: string;
	newPassword: string;
	repeatedNewPassword: string;
}

const ResetPasswordForm = ({ handleSubmit }: ResetPasswordFormProps) => {
	const { register, handleSubmit: handleFormSubmit, formState: { errors } } = useForm<FormData>();

	const validateData = (data: FormData) => {
		if (data.newPassword === data.repeatedNewPassword) {
			handleSubmit(data.oldPassword, data.newPassword);
		}
	};

	return (
		<form className={styles.form} onSubmit={handleFormSubmit(validateData)}>
			<UrfuTextInput
				label="Старый пароль"
				type="password"
				{...register("oldPassword", {
					required: "Старый пароль обязателен",
					minLength: { value: 7, message: "Пароль должен быть не менее 7 символов" }
				})}
				error={errors.oldPassword?.message}
			/>
			<UrfuTextInput
				label="Новый пароль"
				type="password"
				{...register("newPassword", {
					required: "Новый пароль обязателен",
					minLength: { value: 7, message: "Пароль должен быть не менее 7 символов" }
				})}
				error={errors.newPassword?.message}
			/>
			<UrfuTextInput
				label="Повторите новый пароль"
				type="password"
				{...register("repeatedNewPassword", {
					required: "Повторите новый пароль",
					minLength: { value: 7, message: "Пароль должен быть не менее 7 символов" }
				})}
				error={errors.repeatedNewPassword?.message}
			/>
			<UrfuButton type="submit">Сохранить</UrfuButton>
		</form>
	);
};

export default ResetPasswordForm;