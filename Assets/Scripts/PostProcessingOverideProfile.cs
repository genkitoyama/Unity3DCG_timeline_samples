using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

/*
    overrideing parameters for PostProcessingBehaviour with temporary PostProcessingProfile
*/

[ExecuteInEditMode]
    public class PostProcessingOverideProfile : MonoBehaviour {

    public Camera m_Camera;
    private PostProcessingProfile m_tmpProfile = null;
    public PostProcessingProfile m_orgProfile = null;
   
    [Header("DepthOfField")]
    [Min(0.1f), Tooltip("Distance to the point of focus (only used when none is specified in focusTransform).")]
    public float m_FocusDistance;
    [Range(0.05f, 32f), Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
    public float m_Aperture;

    [Header("Bloom")]
    public float m_Intensity;
    public float m_Threshhold;
    [Range(0.00f, 1.0f)]
    public float m_SoftKnee;
    [Range(1.00f, 7.0f)]
    public float m_Radiuds;

    [Header("User Lut")]
    [Range(0f, 1f), Tooltip("Blending factor.")] 
    public float m_Contribution;


    void Start()
    {
    }
    
    void Awake()
    {
        m_Camera = GetComponent<Camera>();
        CreatePostProcessingProfile();
    }

    void InitPrameterFromProfile(PostProcessingProfile profile)
    {
        //depthOfField        
        var settings = profile.depthOfField.settings;
        m_FocusDistance = settings.focusDistance;
        m_Aperture      = settings.aperture ;

        //User Lut
        var lutSettings = profile.userLut.settings;
        m_Contribution =    lutSettings.contribution;

        // Bloom
        var bloomSettings = profile.bloom.settings;
        m_Intensity = bloomSettings.bloom.intensity;
        m_Threshhold = bloomSettings.bloom.threshold;
        m_SoftKnee = bloomSettings.bloom.softKnee;
        m_Radiuds = bloomSettings.bloom.radius;
    }

    void SyncProfile(PostProcessingProfile profile)
    {
        //depthOfField
        var settings = profile.depthOfField.settings;

        settings.focusDistance = m_FocusDistance;
        settings.aperture      = m_Aperture;

        profile.depthOfField.settings = settings;

        //User Lut
 
        var lutSettings = m_tmpProfile.userLut.settings;
        lutSettings.contribution = m_Contribution;

        profile.userLut.settings = lutSettings;


        // Bloom
        var bloomSettings = m_tmpProfile.bloom.settings;
        bloomSettings.bloom.intensity = m_Intensity;
        bloomSettings.bloom.threshold = m_Threshhold;
        bloomSettings.bloom.softKnee = m_SoftKnee;
        bloomSettings.bloom.radius = m_Radiuds;
        profile.bloom.settings = bloomSettings;

    }


    void LateUpdate()
    {
        if(m_tmpProfile != null)
        {
            SyncProfile(m_tmpProfile);

            SetTmpProfile();
        }
    }

    void OnGUI()
    {
        SetOrgProfile();
    }

    void OnEnable()
    {
        CreatePostProcessingProfile();
    }

    void OnDisable()
    {
        SetOrgProfile();        
        
        ReleasePostProcessingProfile();
    }

    void OnDestroy()
    {
        SetOrgProfile();
        if(m_orgProfile != null)
        {
            ReleasePostProcessingProfile();            
        }
    }

    void SetTmpProfile()
    {
        if(m_orgProfile != null)
        {
            PostProcessingBehaviour behaviour = null;
            behaviour = GetComponent<PostProcessingBehaviour>();

            if (behaviour == null)
            {
                return;
            }
            behaviour.profile = m_tmpProfile;       
        }

    }

    void SetOrgProfile()
    {
        if(m_orgProfile != null)
        {
            PostProcessingBehaviour behaviour = null;
            behaviour = GetComponent<PostProcessingBehaviour>();

            if (behaviour == null)
            {
                return;
            }
            behaviour.profile = m_orgProfile;       
        }
    }

    void ReleasePostProcessingProfile()
    {
        m_orgProfile = null;

        if (m_tmpProfile != null)
        {
            DestroyImmediate(m_tmpProfile);
            m_tmpProfile = null;
        }
    }

    void CreatePostProcessingProfile()
    {
        PostProcessingBehaviour behaviour = null;
        behaviour = GetComponent<PostProcessingBehaviour>();

        if (behaviour == null)
        {
            return;
        }

        var profile = behaviour.profile;
        if(profile == null)
        {
            return;
        }

        if (profile != null)
        {
            m_orgProfile = profile;

            m_tmpProfile = ScriptableObject.CreateInstance<PostProcessingProfile>();
            m_tmpProfile = (PostProcessingProfile)Instantiate(profile);
        }

        InitPrameterFromProfile(m_tmpProfile);
    }

}
