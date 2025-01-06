import { useNavigate } from "react-router-dom";
import UserEditForm from "../../../components/UserEditForm/UserEditForm.tsx";
import type {UserEditType} from "../../../components/UserEditForm/userEditModel.ts";
import { useEditSelfMutation, useFetchUserQuery } from "../../../features/api/apiSlice";
import styles from "./SelfEditPage.module.scss";

const SelfEditPage = () => {
    const navigate = useNavigate();
    const { data: user } = useFetchUserQuery();
    const [editSelf] = useEditSelfMutation();

    const handleSelfEdit = async (userData: UserEditType) => {
        await editSelf(userData);
        navigate("/users-page");
    };

    if (user)
        return (
            <div>
                <section className={styles.userEditSection}>
                    <UserEditForm handleSubmit={handleSelfEdit} userData={user} id={user.id} />
                </section>
            </div>
        );
    return <></>;
};

export default SelfEditPage;