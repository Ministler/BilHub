import { useState } from 'react';

export default (props) => {
    const [form, setForm] = useState({});
    const [error, setError] = useState('');
    const onChange = (e, { name, value }) => {
        setForm({ ...form, [name]: value });
        //console.log('name', name);
    };

    const pepe = (event) => {
        setError('');
    };

    const onSubmit = (event) => {
        if (!form.email?.length || !form.password?.length) {
            setError('Please fill the every blank');
            return;
        }

        var users = [];
        users.push({ email: 'a.karakaya@ug.bilkent.edu.tr', password: 'ministler' });
        users.push({ email: 'yusuf.uyar@ug.bilkent.edu.tr', password: 'ministler' });

        for (var i = 0; i < users.length; i++) {
            if (form.email === users[i].email) {
                if (form.password === users[i].password) {
                    props.history.push('/');
                }
            }
        }
        setError('Incorrect email or password.');
    };
    //error = 'Incorrect email or password.';
    //console.log('form', form);
    return { form, onChange, onSubmit, error, pepe };
};
