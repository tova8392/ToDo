import axios from 'axios';
const apiUrl = "https://server-sdrq.onrender.com";

axios.defaults.baseURL = apiUrl;

export default {
  getTasks: async () => {
    try {
      const result = await axios.get(`selectAll`)
      return result.data;
    } catch (err) {
      console.error('Error getting tasks:', err);
    }
  },

  addTask: async (name) => {
    console.log('addTask', name)
    try {
      const result = await axios.post(`add?Name=${encodeURIComponent(name)}`);
      return result.data;
    } catch (err) {
      console.error('Error adding task:', err);
    }
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete });
    try {
      const result = await axios.patch(`update/${id}`, isComplete, {
        headers: {
            'Content-Type': 'application/json'
        }
    });      return result.data;
    } catch (err) {
      console.error('Error setting completion:', err);
    }
  },

  deleteTask: async (id) => {
    console.log('deleteTask', id);
    try {
      const result = await axios.delete(`/delete/${id}`);
      return result.data;
    } catch (err) {
      console.error('Error deleting task:', err);
    }
  }

};