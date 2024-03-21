import axios from 'axios'
import { UserContext, userModel } from 'entities/user'
import React, { useContext } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAxios } from 'shared/hooks'
import { UserEditForm, UserEditType } from 'widgets/user-edit-form'

import styles from './SelfEditPage.module.scss'

const SelfEditPage = () => {
    const navigate = useNavigate()
    const {response, loading, error} = useAxios<userModel.User>(`https://gost-storage.ru/api/accounts/self-info`)

    const handleSelfEdit = (userData: UserEditType) => {
        axios.post('https://gost-storage.ru/api/accounts/self-edit', userData, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
            }
        }).then(() => navigate('/users-page'))
    }
    
    if(response) 
    return (
        <div>
            <section className={styles.userEditSection}>
                <UserEditForm handleSubmit={handleSelfEdit} userData={response} id={response.id}/>
            </section>
        </div>
    )
    else return <></>
}

export default SelfEditPage