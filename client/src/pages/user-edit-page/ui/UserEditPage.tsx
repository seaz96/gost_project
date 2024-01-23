import React, { useContext } from 'react'

import styles from './UserEditPage.module.scss'
import { UserEditForm, UserEditType } from 'widgets/user-edit-form';
import { useAxios } from 'shared/hooks';
import { UserContext, userModel } from 'entities/user';
import { useParams } from 'react-router-dom';
import axios from 'axios';


const UserEditPage = () => {
    const {user} = useContext(UserContext)
    const id = parseInt(useParams().id || '')
    const {response, loading, error} = useAxios<userModel.User>(`https://backend-seaz96.kexogg.ru/api/accounts/get-user-info`, {'id': id})

    if(loading) return <></>

    const handleUserEdit = (userData: UserEditType) => {
        const editedUser = {'login': userData.name, 'org_name': userData.orgName, 'org_branch': userData.orgBranch, 'org_activity': userData.orgActivity}
        axios.post('https://backend-seaz96.kexogg.ru/api/accounts/admin-edit', editedUser, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
            }
        })
        const isAdmin = response?.role === 'Admin' || response?.role === 'Heisenberg'
        if(userData.is_admin !== isAdmin) {
            axios.post('https://backend-seaz96.kexogg.ru/api/accounts/admin-edit', 
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
    }
 
    const handleSelfEdit = (userData: UserEditType) => {
        console.log(userData)
        const editedUser = {'login': userData.name, 'org_name': userData.orgName, 'org_branch': userData.orgBranch, 'org_activity': userData.orgActivity}
        axios.post('https://backend-seaz96.kexogg.ru/api/accounts/self-edit', editedUser, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('jwt_token')}`
            }
        }).then(repsonse => console.log(repsonse))
    }
    
    if(response) 
    return (
        <div>
            <section className={styles.userEditSection}>
                <UserEditForm handleSubmit={id && id === user?.id ? handleSelfEdit : handleUserEdit } userData={response}/>
            </section>
        </div>
    )
    else return <></>
}

export default UserEditPage;