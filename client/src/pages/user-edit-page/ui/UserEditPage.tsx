import styles from './UserEditPage.module.scss'
import { UserEditForm, UserEditType } from 'widgets/user-edit-form';
import { useAxios } from 'shared/hooks';
import { userModel } from 'entities/user';
import { useNavigate, useParams } from 'react-router-dom';
import { axiosInstance } from 'shared/configs/axiosConfig';


const UserEditPage = () => {
    const navigate = useNavigate()
    const id = parseInt(useParams().id || '')
    const {response, loading, error} = useAxios<userModel.User>(`/accounts/get-user-info`, {'id': id})
    if(loading) return <></>

    const handleUserEdit = (userData: UserEditType) => {
        axiosInstance.post('/accounts/admin-edit', userData)
        .then(() => {
            const isAdmin = response?.role === 'Admin' || response?.role === 'Heisenberg'
            if(userData.is_admin !== isAdmin) {
                axiosInstance.post('/accounts/make-admin', 
                {
                    'userId': response?.id,
                    'isAdmin': userData.is_admin && !isAdmin ? true : false
                })
            }
        })
        .then(() => navigate('/users-page'))
    }
 

    
    if(response) 
    return (
        <div>
            <section className={styles.userEditSection}>
                <UserEditForm handleSubmit={handleUserEdit } userData={response} id={response.id}/>
            </section>
        </div>
    )
    else return <></>
}

export default UserEditPage;