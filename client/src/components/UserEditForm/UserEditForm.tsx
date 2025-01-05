import type React from "react";
import { useContext, useState } from "react";

import { UserContext, type userModel } from "../../entities/user";
import { Button, Input } from "../../shared/components";
import styles from "./UserEditForm.module.scss";
import type { UserEditType } from "./userEditModel.ts";

interface UserEditFormProps {
	handleSubmit: Function;
	userData: userModel.User;
	id: number;
}

const UserEditForm: React.FC<UserEditFormProps> = (props) => {
	const { handleSubmit, userData, id } = props;
	const { user } = useContext(UserContext);

	const [userEditData, setUserEditData] = useState<UserEditType>({
		name: userData.name,
		login: userData.login,
		org_name: userData.orgName,
		org_activity: userData.orgActivity,
		org_branch: userData.orgBranch,
		is_admin: userData.role === "Admin" || userData.role === "Heisenberg" ? true : false,
	});

	return (
		<form
			className={styles.form}
			onSubmit={(event) => {
				event.preventDefault();
				handleSubmit(userEditData);
			}}
		>
			<Input
				label="ФИО пользователя"
				type="text"
				value={userEditData.name}
				onChange={(value: string) => setUserEditData({ ...userEditData, name: value })}
			/>
			<Input
				label="Логин"
				type="text"
				value={userEditData.login}
				onChange={(value: string) => setUserEditData({ ...userEditData, login: value })}
			/>
			<Input
				label="Название организации"
				type="text"
				value={userEditData.org_name}
				onChange={(value: string) => setUserEditData({ ...userEditData, org_name: value })}
			/>
			<Input
				label="Отделение организации"
				type="text"
				value={userEditData.org_branch}
				onChange={(value: string) => setUserEditData({ ...userEditData, org_branch: value })}
			/>
			<Input
				label="Деятельность организации"
				type="text"
				value={userEditData.org_activity}
				onChange={(value: string) => setUserEditData({ ...userEditData, org_activity: value })}
			/>
			{id !== user?.id && user?.role === "Heisenberg" && (
				<div className={styles.checkboxContainer}>
					<input
						type="checkbox"
						id="switchAdmin"
						onChange={() =>
							setUserEditData({
								...userEditData,
								is_admin: !userEditData.is_admin,
							})
						}
						checked={userEditData.is_admin ? true : false}
					/>
					<label htmlFor="switchAdmin">Пользователь является администратором</label>
				</div>
			)}
			<Button type="submit" onClick={() => {}} className={styles.formButton} isFilled>
				Сохранить
			</Button>
		</form>
	);
};

export default UserEditForm;
