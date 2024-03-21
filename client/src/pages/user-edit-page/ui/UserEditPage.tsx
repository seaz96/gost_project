import React, { useContext } from 'react'

import styles from './UserEditPage.module.scss'
import { UserEditForm, UserEditType } from 'widgets/user-edit-form';
import { useAxios } from 'shared/hooks';
import { UserContext, userModel } from 'entities/user';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';


const UserEditPage = () => {
    const navigate = useNavigate()
    const id = parseInt(useParams().id || '')
    const {response, loading, error} = useAxios<userModel.User>(`https://gost-storage.ru/api/accounts/get-user-info`, {'id': id})
    if(loading) return <></>

    const handleUserEdit = (userData: UserEditType) => {
        axios.post('https://gost-storage.ru/api/accounts/admin-edit', userData, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
            }
        })
        .then(() => {
            const isAdmin = response?.role === 'Admin' || response?.role === 'Heisenberg'
            if(userData.is_admin !== isAdmin) {
                axios.post('https://gost-storage.ru/api/accounts/make-admin', 
                {
                    'userId': response?.id,
                    'isAdmin': userData.is_admin && !isAdmin ? true : false
                }, 
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
                    }
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