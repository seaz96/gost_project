import React, { useState } from 'react'

import styles from './UserEditForm.module.scss'
import { Button, Input } from 'shared/components';
import { UserEditType } from '../model/userEditModel';
import { userModel } from 'entities/user';

interface UserEditFormProps {
    handleSubmit: Function,
    userData: userModel.User
}

const UserEditForm: React.FC<UserEditFormProps> = props => {
    const {
        handleSubmit,
        userData
    } = props

    const [userEditData, setUserEditData] = useState<UserEditType>({
        name: userData.name,
        orgName: userData.orgName,
        orgActivity: userData.orgActivity,
        orgBranch: userData.orgBranch,
        is_admin: userData.role === 'Admin' ? true : false
    })

    return (
        <form className={styles.form} onSubmit={(event) => {
            event.preventDefault();
            handleSubmit(userEditData)
        }}>
            <Input
                label='ФИО пользователя'
                type='text'
                value={userEditData.name}
                onChange={(value: string) => setUserEditData({...userEditData, name: value})}
            />
            <Input 
                label='Название организации' 
                type='text'
                value={userEditData.orgName}
                onChange={(value: string) => setUserEditData({...userEditData, orgName: value})}
            />
            <Input 
                label='Отделение организации' 
                type='text'
                value={userEditData.orgActivity}
                onChange={(value: string) => setUserEditData({...userEditData, orgActivity: value})}
            />
            <Input 
                label='Деятельность организации' 
                type='text'
                value={userEditData.orgBranch}
                onChange={(value: string) => setUserEditData({...userEditData, orgBranch: value})}
            />
            {userData.id !== 0 && userData.role === 'Heisenberg' &&
                <div className={styles.checkboxContainer}>
                    <input type='checkbox' id='switchAdmin' checked={userEditData.is_admin ? true : false}/>
                    <label htmlFor='switchAdmin'>Пользователь  является администратором</label>
                </div>
            }
            <Button type='submit' onClick={() => {}} className={styles.formButton} isFilled>Сохранить</Button>
        </form>
    )
}

export default UserEditForm;